using Skill;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{

    public class Player : Character
    {

       // private RaycastHit hitInfo;
       // private Vector3 mouseDir;
        private float xInput;
        private float zInput;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        private static readonly int IsRun = Animator.StringToHash("isRun");
        private Collider[] itemColliders;
        private KeyCode key;
        private Transform _target;
        private Vector3 characterMovingDir;
        public new Transform target
        {
            get
            {
                return _target;
            }
            set
            {
                CinemachineManager.Instance.playerCam.gameObject.SetActive(!value);
                CinemachineManager.Instance.bossCam.gameObject.SetActive(value);
                _target = CinemachineManager.Instance.bossCam.LookAt = value;
            }
        }

        private Projectile.ProjectileInfo chargeProjectileInfo;
        public bool onCharge;
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
        public Transform curCam;
        public Transform fixCam;
        private float cameraSpeed ;
        private float NoneTargetEleapse;

        protected override void Awake()
        {
            base.Awake();
            colliders = new Collider[8];
            itemColliders = new Collider[1];
            actives = new List<Skill.Skill>();
            chargeProjectileInfo = new Projectile.ProjectileInfo(layerMask,
                ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet,  (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(point,
                        5 + range * 0.2f, colliders,
                        layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out targetCharacter);
                        if (targetCharacter)
                            targetCharacter.Hit(point, 25 + dmg * 2f,0);
                    }
                });

            cameraSpeed = 300;
        }
        protected override void Start()
        {
            base.Start();
            //퀵슬롯 구현후 삭제
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Roll]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Grenade]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.GravityBomb]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.SpiderMine]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Hyperion]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Boomerang]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Distortion]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.AegisBarrier]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.MassShooting]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Block]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Stimpack]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Smash]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Gardian]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Charge]);

            hpBar.TryGetComponent(out RectTransform hpRect);
            
            hpRect.anchoredPosition = new Vector2(0, -Screen.height*2/5);
        }
        protected void Update()
        {
            Vector3 position = thisCurTransform.position;
            
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            InputDir = new Vector3(xInput, 0, zInput);

            var rotInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            var rot = transform.eulerAngles;
            rot.y += rotInput.x * cameraSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rot);
            
            

            BaseUpdate();
            if (dying)
                return;

            if (Physics.OverlapSphereNonAlloc(position, 1f, itemColliders, 1 << 7) > 0)
            {
                itemColliders[0].TryGetComponent(out Item.Item getItem);
                getItem.Use(this);
            }
            if (onSkill is not null && onSkill.skillInfo.clipLayer == 2)
                return;
            #region MovingMan

        
            if (xInput != 0 || zInput != 0)
            {
                if (xInput is > 0.75f or < -0.75f && zInput is > 0.75f or < -0.75f)
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
                }
                else
                    anim.SetBool(IsRun, isRun = false);
                InputDir = transform.rotation *InputDir;
                characterMovingDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position + InputDir));
                
                anim.SetFloat(X, characterMovingDir.x);
                anim.SetFloat(Z, characterMovingDir.z);

                thisCurTransform.position += InputDir * (Time.deltaTime * speed * (isRun ? 1.5f : 1));
            }
            /*
             * 
             * 
             */
            if (isRun)
            {
                fixCam = curCam;
            }
            else
            {

            }

            thisCurTransform.forward =
                Vector3.RotateTowards(thisCurTransform.forward, isRun ? InputDir :
                    target ? target.position - position : thisCurTransform.forward, 6 * Time.deltaTime, 0);


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
                        float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[i].transform.position - position, Vector3.up);

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
                float angle = Vector3.SignedAngle(thisCurTransform.forward, target.position - position, Vector3.up);

                if ((angle < 0 ? -angle : angle) > viewAngle || Vector3.Distance(target.position, thisCurTransform.position) > range + .5f)
                {
                    NoneTargetEleapse += Time.deltaTime;
                    if(NoneTargetEleapse>2.5f)
                    {
                        NoneTargetEleapse -= 2.5f;
                        anim.SetBool(onTarget, target = null);
                    }
                }
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Q))
            {
                actives[0].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {

                actives[1].Use(this);
            }else if (Input.GetKeyDown(KeyCode.R))

            {
                actives[2].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Space))
            {
                actives[13].Use(this);
                Debug.Log(onSkill);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                actives[3].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                actives[4].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {

                actives[5].Use(this);
            } else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                actives[6].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                actives[7].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {

                actives[8].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                actives[9].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                actives[10].Use(this);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                actives[11].Use(this);
            }else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                actives[12].Use(this);
            }

        }

        protected override void Attack()
        {

            Vector3 forward = thisCurTransform.forward;
            if (onCharge)
            {
                SpawnManager.Instance.Launch(thisCurTransform.position,forward,0 ,1+duration*0.5f, 20+speed*2,range*0.5f, ref chargeProjectileInfo);

                impact -= (45 + dmg * 0.5f) * 0.1f * forward;
                onCharge = false;
            }
                
            else
                SpawnManager.Instance.Launch(thisCurTransform.position,forward,
                    dmg ,1+duration*0.5f, 20+speed*2,range*0.5f, ref projectileInfo);

        }
    }
}
