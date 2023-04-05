using Skill;
using UnityEngine;

namespace Character
{

    public class Player : Character
    {
        [SerializeField]private GameObject bulletPrefab;
        private RaycastHit hitInfo;
        private Vector3 mouseDir;
        private float xInput;
        private float zInput;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        private static readonly int onTarget = Animator.StringToHash("onTarget");

        private Collider[] itemColliders;
        public Vector3 InputDir { get; set; }
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
            //퀵슬롯 구현후 삭제
            actives.Add(ResourceManager.Instance.skills[0]);
            actives.Add(ResourceManager.Instance.skills[1]);
            actives.Add(ResourceManager.Instance.skills[3]);
            hpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position + Vector3.up * 2f);
        }
        protected void Update()
        {
            Vector3 position = thisCurTransform.position;
            Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo,Mathf.Infinity,layerMask:1<<0);
            mouseDir = hitInfo.point - position;
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            InputDir = new Vector3(xInput, 0, zInput);
            BaseUpdate();
            if (dying)
                return;
            mainCam.transform.position = position + new Vector3(0, 25, -27.5f);
            
            if (Physics.OverlapSphereNonAlloc(position, 1f, itemColliders, 1 << 7) > 0)
            {
                itemColliders[0].TryGetComponent(out Item.Item getItem);
                getItem.Use(this);
            }
            if (onSkill is not null && onSkill.skillInfo.clipLayer==2)
                return;
            #region MovingMan

            var vector3 = InputDir;
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
                actives[0].Use(this);
            }else if (Input.GetKeyDown(KeyCode.E))
            {
                actives[1].Use(this);
            }else if (Input.GetKeyDown(KeyCode.R))
            {
                actives[2].Use(this);
            }
            

        }

        protected override void Attack()
        {
            SpawnManager.Instance.Launch(thisCurTransform.position,thisCurTransform.forward,gameObject.layer,dmg,ProjectileType.Bullet1);
        }
    }
}
