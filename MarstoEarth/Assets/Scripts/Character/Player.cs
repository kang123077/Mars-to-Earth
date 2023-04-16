using Skill;
using UnityEngine;
using System.Collections.Generic;

namespace Character
{

    public class Player : Character
    {

        private RaycastHit hitInfo;
        private Vector3 mouseDir;
        private float xInput;
        private float zInput;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        private Collider[] itemColliders;
        private KeyCode  key;
        private LayerMask obstacleMask;

        protected List<Skill.Skill> actives;
        private KeyCode[] keys = new[]
        {
            KeyCode.UpArrow,
            KeyCode.RightArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.W,
            KeyCode.D,
            KeyCode.S,
            KeyCode.A
        };


        private bool isRun;
        private float lastInputTime;
        
        public Vector3 InputDir;
        private static readonly int IsRun = Animator.StringToHash("isRun");

        protected override void Awake()
        {
            base.Awake();
            colliders = new Collider[8];
            itemColliders = new Collider[1];
            anim.SetFloat(movingSpeed, 1 + speed * 0.1f);
            
            actives = new List<Skill.Skill>();
            //layerMask = (1 << LayerMask.NameToLayer("Obstacle"));
            //layerMask = ~layerMask;
        }
        protected override void Start()
        {
            base.Start();
            //퀵슬롯 구현후 삭제
            actives.Add(ResourceManager.Instance.skills[0]);
            actives.Add(ResourceManager.Instance.skills[1]);
            actives.Add(ResourceManager.Instance.skills[2]);
            actives.Add(ResourceManager.Instance.skills[3]);
            actives.Add(ResourceManager.Instance.skills[4]);
            actives.Add(ResourceManager.Instance.skills[5]);
            actives.Add(ResourceManager.Instance.skills[6]);
            actives.Add(ResourceManager.Instance.skills[7]);
            actives.Add(ResourceManager.Instance.skills[8]);
            actives.Add(ResourceManager.Instance.skills[9]);
            actives.Add(ResourceManager.Instance.skills[10]);
            actives.Add(ResourceManager.Instance.skills[11]);
            actives.Add(ResourceManager.Instance.skills[12]);

            hpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position + Vector3.up * 2f);
        }
        protected void Update()
        {
            Vector3 position = thisCurTransform.position;
            Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity,1<<0);
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

            
            if (xInput != 0 || zInput != 0)
            {
                if (xInput is > 0.75f or < -0.75f && zInput is >0.75f or<-0.75f)
                {
                    InputDir.x *=  0.71f;
                    InputDir.z *=  0.71f;

                }
                
                if (Input.anyKey)
                {
                    foreach (KeyCode keyCode in keys)
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            if (Time.time - lastInputTime < 0.3f && key == keyCode)
                            {   //연속으로 두번왓는지 확인
                                anim.SetBool(IsRun, isRun = true);
                            }
                            key = keyCode;           
                            lastInputTime = Time.time;
                            break;
                        }
                    }
                }else
                    anim.SetBool(IsRun, isRun = false);

                thisCurTransform.position += InputDir * (Time.deltaTime * speed * (isRun?1.5f:1));
            }
            
            thisCurTransform.forward =
                Vector3.RotateTowards(thisCurTransform.forward, isRun? InputDir:
                    target? target.position-position : mouseDir, 6 * Time.deltaTime, 0);
            
            Vector3 characterDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position + InputDir));

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

                        if ((angle < 0 ? -angle : angle) < viewAngle)
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
                

                
                if ((angle < 0 ? -angle : angle) > viewAngle || Vector3.Distance(target.position, thisCurTransform.position) > range + .5f)
                {
                    anim.SetBool(onTarget, target = null);
                }
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Q))
            {
                actives[0].Use(this);
            }else if (Input.GetKeyDown(KeyCode.E))
            {
                actives[4].Use(this);
            }else if (Input.GetKeyDown(KeyCode.R))

            {
                actives[5].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Space))
            {
                actives[7].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                actives[8].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                actives[2].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                actives[6].Use(this);
            } else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                actives[9].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                actives[10].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                actives[11].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                actives[12].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                actives[1].Use(this);
            }

        }

        protected override void Attack()
        {
            if (onSkill is ChargeShotSkill)
                    SkillEffect();
            else
                SpawnManager.Instance.Launch(thisCurTransform.position,thisCurTransform.forward,
                    dmg ,1+duration*0.5f, 20+speed*2,range*0.5f, ref projectileInfo);

        }
    }
}
