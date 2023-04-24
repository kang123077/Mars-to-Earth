using Skill;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class Player : Character
    {
        public Vector3 InputDir;
        public Transform camPoint;

        public new Transform target
        {
            get => _target;
            set
            {
                CinemachineManager.Instance.playerCam.gameObject.SetActive(!value);
                CinemachineManager.Instance.bossCam.gameObject.SetActive(value);
                _target = CinemachineManager.Instance.bossCam.LookAt = value;
            }
        }

        private Transform _target;
        [SerializeField] public Transform muzzle;

        [HideInInspector] public bool onCharge;

        protected List<Skill.Skill> actives;

        private float xInput;
        private float zInput;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        private static readonly int IsRun = Animator.StringToHash("isRun");
        private Collider[] itemColliders;
        private Projectile.ProjectileInfo chargeProjectileInfo;

        private Vector3 characterMovingDir;
        private KeyCode key;

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

        private float hitScreenAlphaValue;
        private UnityEngine.UI.Image hitScreen;
        private Color hitScreenColor;

        private Vector3 repoterForward;
        private Vector3 targetDir;

        protected override void Awake()
        {
            base.Awake();
            colliders = new Collider[8];
            itemColliders = new Collider[1];
            actives = new List<Skill.Skill>();

            chargeProjectileInfo = new Projectile.ProjectileInfo(layerMask,
                ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet, (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(point,
                        5 + range * 0.2f, colliders,
                        layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out targetCharacter);
                        if (targetCharacter)
                            targetCharacter.Hit(point, 25 + dmg * 2f, 0);
                    }
                });
            hitScreen = ((CombatUI)UIManager.Instance.UIs[(int)UIType.Combat]).hitScreen;
            hitScreenColor = hitScreen.color;
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

            hpBar = ((CombatUI)UIManager.Instance.UIs[(int)UIType.Combat]).playerHP;
        }

        protected void Update()
        {
            try
            {
                BaseUpdate();
                Vector3 position = thisCurTransform.position;

                xInput = Input.GetAxis("Horizontal");
                zInput = Input.GetAxis("Vertical");
                InputDir = new Vector3(xInput, 0, zInput);

                if (hitScreenAlphaValue > 0)
                {
                    hitScreenAlphaValue -= Time.deltaTime * hp * (1 / characterStat.maxHP);
                    hitScreenColor.a = hitScreenAlphaValue;
                    hitScreen.color = hitScreenColor;
                }

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
                        InputDir.x *= 0.71f;
                        InputDir.z *= 0.71f;
                    }

                    if (Input.anyKey)
                    {
                        foreach (KeyCode keyCode in keys)
                        {
                            if (Input.GetKeyDown(keyCode))
                            {
                                if (Time.time - lastInputTime < 0.3f && key == keyCode)
                                {
                                    //연속으로 두번왓는지 확인
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

                    InputDir = CinemachineManager.Instance.follower.rotation * InputDir;
                    thisCurTransform.position += InputDir * (Time.deltaTime * speed * (isRun ? 1.5f : 1f));

                    Vector3 lowerDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position + InputDir));

                    anim.SetFloat(X, lowerDir.x);
                    anim.SetFloat(Z, lowerDir.z);
                }


                if (target)
                {
                    Vector3 targetPos = target.position;
                    targetPos.y = 0;
                    targetDir = targetPos - thisCurTransform.position;
                }

                repoterForward = CinemachineManager.Instance.follower.forward;
                repoterForward.y = 0;

                thisCurTransform.forward =
                    Vector3.RotateTowards(thisCurTransform.forward,
                        isRun ? InputDir : target ? targetDir : repoterForward, Time.deltaTime * speed * 2f, 0);

                #endregion

                #region Targeting

                if (Input.GetMouseButtonDown(0))
                    anim.SetTrigger(attacking);
                float minCoLength = 1000;
                if (!target)
                {
                    int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, range - 1, colliders,
                        1 << 6);

                    for (int i = 0; i < size; i++)
                    {
                        float angle = Vector3.SignedAngle(repoterForward, colliders[i].transform.position - position,
                            Vector3.up);
                        if ((angle < 0 ? -angle : angle) < viewAngle - 5)
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
                else
                {
                    Vector3 repoterPosition = CinemachineManager.Instance.follower.position;
                    repoterPosition.y = 1;
                    int size = Physics.OverlapCapsuleNonAlloc(repoterPosition,
                        repoterPosition + repoterForward * range, 0.5f, colliders, layerMask);
                    switch (size)
                    {
                        case 1:
                            target = colliders[0].transform;
                            break;
                        case > 1:
                        {
                            for (int i = 0; i < size; i++)
                            {
                                float coLeng = Vector3.Distance(colliders[i].transform.position, position);
                                if (minCoLength > coLeng)
                                {
                                    minCoLength = coLeng;
                                    target = colliders[i].transform;
                                }
                            }

                            break;
                        }
                        default:
                        {
                            float angle = Vector3.SignedAngle(repoterForward, target.position - position, Vector3.up);
                            if ((angle < 0 ? -angle : angle) > viewAngle + 5 ||
                                Vector3.Distance(target.position, thisCurTransform.position) > range + 1)
                                anim.SetBool(onTarget, target = null);
                            break;
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
                }
                else if (Input.GetKeyDown(KeyCode.R))

                {
                    actives[2].Use(this);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
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
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
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
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    actives[9].Use(this);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    actives[10].Use(this);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    actives[11].Use(this);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    actives[12].Use(this);
                }
            }
            catch (System.Exception)
            {
                //
            }
        }

        protected override void Attack()
        {
            Vector3 muzzleForward = muzzle.forward;
            if (onCharge)
            {
                SpawnManager.Instance.Launch(muzzle.position, muzzleForward, 0, 1 + duration * 0.5f, 20 + speed * 2,
                    range * 0.5f, ref chargeProjectileInfo);

                impact -= (45 + dmg * 0.5f) * 0.1f * muzzleForward;
                onCharge = false;
            }

            else
                SpawnManager.Instance.Launch(muzzle.position, muzzleForward,
                    dmg, 1 + duration * 0.5f, 20 + speed * 2, range * 0.5f, ref projectileInfo);
        }

        protected internal override void Hited(Vector3 attacker, float dmg, float penetrate = 0)
        {
            hitScreenAlphaValue += dmg * 2 * (1 / characterStat.maxHP);
            base.Hited(attacker, dmg, penetrate);
        }
    }
}