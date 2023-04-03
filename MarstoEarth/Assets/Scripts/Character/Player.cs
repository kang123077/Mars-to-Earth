using Skill;
using UnityEngine;

namespace Character
{

    public class Player : Character
    {
        [SerializeField]private GameObject bulletPrefab;
        private RaycastHit hitInfo;
        private Vector3 mouseDir;
        public Vector3 inputDir { get; set; }
        private float xInput;
        private float zInput;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        
        private static readonly int onTarget = Animator.StringToHash("onTarget");
        private float _EXP;
        public float EXP
        {
            get => _EXP;
            set
            {
                float curValue=value;
                while (curValue>=100) {
                    curValue -= 100;
                    level++;
                };
                _EXP = curValue;
            }
        }
        
        public float optanium { get; set; }

        private Collider[] itemColliders;

        protected override void Awake()
        {
            base.Awake();
            colliders = new Collider[8];
            itemColliders = new Collider[1];
            anim.SetFloat(movingSpeed,1+speed*0.4f);
        }
        protected override void Start()
        {
            base.Start();
            hpBar.transform.position = mainCam.WorldToScreenPoint(((Component)this).transform.position + Vector3.up * 2f);
        }
        protected void Update()
        {
            
            
            Vector3 position = thisCurTransform.position;
            Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo,Mathf.Infinity,layerMask:1<<0);
            mouseDir = hitInfo.point - position;
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            inputDir = new Vector3(xInput, 0, zInput);
            BaseUpdate();
            if (dying)
                return;
            mainCam.transform.position = position + new Vector3(0, 25, -27.5f);
            
            if (Physics.OverlapSphereNonAlloc(position, 1f, itemColliders, 1 << 7) > 0)
            {
                itemColliders[0].TryGetComponent(out Item.Item getItem);
                getItem.Use(this);
            }
            
            #region MovingMan

            var vector3 = inputDir;
            if (xInput != 0 || zInput != 0)
            {
                if (xInput != 0 && zInput != 0)
                {
                    vector3.x *=  0.7f;
                    vector3.z *=  0.7f;
                }
                thisCurTransform.position += vector3 * (Time.deltaTime * speed);
            }
            thisCurTransform.forward =
                Vector3.RotateTowards(thisCurTransform.forward, target? target.position-position :
                    mouseDir, 6 * Time.deltaTime, 0);
            Vector3 characterDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position + vector3));
            anim.SetFloat(X, characterDir.x);
            anim.SetFloat(Z, characterDir.z);
            #endregion
            
            #region Targeting
            if (Input.GetMouseButtonDown(0))
                anim.SetTrigger(attacking);
            if (!target)
            {
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, range, colliders,
                    1 << 6);
                if (size > 0)
                {
                    float minCoLength = 1000;
                    for (int i = 0; i < size; i++)
                    {
                        float angle = Vector3.SignedAngle(mouseDir, colliders[i].transform.position - position,
                            Vector3.up);
                        
                        if ((angle<0?-angle:angle) < viewAngle)
                        {
                            float coLeng = Vector3.Distance(colliders[i].transform.position, position);
                            if (minCoLength > coLeng)
                            {
                                minCoLength = coLeng;
                                target = colliders[i].transform;
                            }
                        }
                    }
                    anim.SetBool(onTarget, target);
                }
            }
            else  
            {
                float angle = Vector3.SignedAngle(mouseDir, target.position - position, Vector3.up);
                if ((angle < 0 ? -angle : angle) > viewAngle||Vector3.Distance(target.position, thisCurTransform.position) > range + .5f)

                {
                    anim.SetBool(onTarget, target = null);
                    thisCurTransform.forward =
                        Vector3.RotateTowards(thisCurTransform.forward, mouseDir, Time.deltaTime * 10, 10);
                }
            }
            #endregion
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Á¢±Ù");
                registActives[0].Use(this, 3);
            }else if (Input.GetKeyDown(KeyCode.E))
            {
                //registActives[1].Use();
            }else if (Input.GetKeyDown(KeyCode.E))
            {
                //registActives[2].Use();
            }
            

        }

        protected override void Attack()
        {
            SpawnManager.Instance.Launch(transform,gameObject.layer,dmg,bulletPrefab);
        }
    }
}
