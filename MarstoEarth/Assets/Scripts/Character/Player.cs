using Skill;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Player : Character
    {
        public Vector3 InputDir;
        public Transform camPoint;
        public bool isInsidePath;
        public override bool stun
        {
            get => _stun;
            set
            {
                base.stun = value;
                anim.SetBool(IsRun, !value && isRun);
                _stun = value;
            }
        }
        public override Transform target
        {
            get => base.target;
            set
            {
                CinemachineManager.Instance.playerCam.gameObject.SetActive(!value);
                CinemachineManager.Instance.bossCam.gameObject.SetActive(value);
                base.target = CinemachineManager.Instance.bossCam.LookAt = value;
            }
        }


        protected List<Skill.Skill> actives;

        private float xInput;
        private float zInput;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        private static readonly int IsRun = Animator.StringToHash("isRun");
        private Collider[] itemColliders;

        private KeyCode key;

        private KeyCode[] moveKeys = new[]
        {
            KeyCode.UpArrow,
            KeyCode.RightArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.W,
            KeyCode.D,
            KeyCode.S,
            KeyCode.A,
        };
        private KeyCode[] skillKeys = new[]
        {
            KeyCode.Q,
            KeyCode.E,
            KeyCode.R,
            KeyCode.Space
        };

        private bool _isRun;

        public bool isRun
        {
            get => _isRun;
            set
            {
                anim.SetBool(IsRun, value);
                AudioManager.Instance.PlayEffect(value ? (int)CombatEffectClip.run : (int)CombatEffectClip.walk, step);
                _isRun = value;
            }
        }
        private float lastInputTime;

        private float hitScreenAlphaValue;
        private UnityEngine.UI.Image hitScreen;
        private Color hitScreenColor;

        private Vector3 repoterForward;
        private Vector3 targetDir;

        public ParticleSystem[] effects;

        public float bulletSpeed;

        protected override void Awake()
        {
            base.Awake();
            colliders = new Collider[8];
            itemColliders = new Collider[1];
            actives = new List<Skill.Skill>();
            isInsidePath = false;
            bulletSpeed = 35 + speed * 2;
        }

        protected override void Start()
        {
            base.Start();
            //테스트용
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
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Gardian]);
            actives.Add(ResourceManager.Instance.skills[(int)SkillName.Charge]);
            foreach (var a in actives)
            {
                a.Init(this);
            }
            //테스트용 actives
            hpBar = combatUI.playerHP;
            hitScreen = combatUI.hitScreen;
            hitScreenColor = hitScreen.color;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override bool AddBuff(SPC buff)
        {
            if (!base.AddBuff(buff)) return false;
            combatUI.ConnectSPCImage(buff.icon);
            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override int RemoveBuff(SPC buff)
        {
            int findIndex = base.RemoveBuff(buff);
            Destroy(combatUI.SPCSlots[findIndex].gameObject);
            combatUI.SPCSlots.RemoveAt(findIndex);
            return -1;
        }
        protected override bool BaseUpdate()
        {
            if (!base.BaseUpdate())
                return false;
            for (buffElementIdx = 0; buffElementIdx < Buffs.Count; buffElementIdx++)
                combatUI.SPCSlots[buffElementIdx].fillAmount = Buffs[buffElementIdx].currentTime * (1 / Buffs[buffElementIdx].duration);

            if (!stun && hitScreenAlphaValue > 0)
            {
                hitScreenAlphaValue -= Time.deltaTime * hp * (1 / characterStat.maxHP);
                hitScreenColor.a = hitScreenAlphaValue;
                hitScreen.color = hitScreenColor;
            }

            return !stun;
        }
        protected void Update()
        {
            if (!BaseUpdate())
                return;
            Vector3 position = thisCurTransform.position;

            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            InputDir = new Vector3(xInput, 0, zInput);

            if (Physics.OverlapSphereNonAlloc(position, 1f, itemColliders, 1 << 7) > 0)
            {
                itemColliders[0].TryGetComponent(out Item.Item getItem);
                getItem.Use(this);
            }

            if (onSkill is not null && onSkill.skillInfo.clipLayer == 2)
                return;

            #region MovingMan


            if (xInput is < 0.1f and > -0.1f && zInput is < 0.1f and > -0.1f)
            {
                step.volume = 0;
                if(isRun)
                    isRun = false;
            }
            else
                step.volume = 0.8f;
            
            if (xInput != 0 || zInput != 0)
            {
                if (xInput is > 0.75f or < -0.75f && zInput is > 0.75f or < -0.75f)
                {
                    InputDir.x *= 0.71f;
                    InputDir.z *= 0.71f;
                }

                if (Input.anyKey)
                {
                    foreach (KeyCode keyCode in moveKeys)
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            if (Time.time - lastInputTime < 0.3f && key == keyCode)
                                if (onSkill is not MassShootingSkill)
                                    isRun = true;

                            key = keyCode;
                            lastInputTime = Time.time;
                            break;
                        }
                    }
                }
                else
                    isRun = false;
                
                InputDir = CinemachineManager.Instance.follower.rotation * InputDir;
                thisCurTransform.position += InputDir * (Time.deltaTime * speed * (isRun ? 1.5f : 1f));

                Vector3 lowerDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position + InputDir));

                anim.SetFloat(X, lowerDir.x);
                anim.SetFloat(Z, lowerDir.z);
            }

            repoterForward = CinemachineManager.Instance.follower.forward;
            repoterForward.y = 0;

            #endregion
            #region Targeting

            if (Input.GetMouseButtonDown(0))
                anim.SetTrigger(attacking);

            int size = Physics.OverlapSphereNonAlloc(position, sightLength - 1, colliders,
                layerMask);
            float minAngle = 180;
            float angle;
            for (int i = 0; i < size; i++)
            {
                angle = Mathf.Acos(Vector3.Dot(repoterForward, (colliders[i].transform.position - position).normalized)) * Mathf.Rad2Deg;

                angle = angle < 0 ? -angle : angle;
                if (angle < viewAngle - 5)
                {
                    if (minAngle > angle)
                    {
                        minAngle = angle;
                        if (!isInsidePath)
                        {
                            target = colliders[i].transform;
                        }
                    }
                }
            }
           
            float targetDist = 0;
            if (target)
            {
                Vector3 targetPos = target.position;
                targetPos.y = 0;
                var velocity = ((Monster)targetCharacter).ai.velocity;
                if (velocity.x is > 0.1f or <-0.1f||velocity.z is > 0.1f or <-0.1f)
                {
                    targetDist = Vector3.Distance(targetPos, position);
                    targetPos += velocity * (targetDist*(1/bulletSpeed));
                }
                targetDir = targetPos - position;
            }
            if (minAngle > 179 && target)
            {
                angle = Mathf.Acos(Vector3.Dot(repoterForward, targetDir.normalized)) * Mathf.Rad2Deg;

                angle = angle < 0 ? -angle : angle;
                if ((angle < 0 ? -angle : angle) > viewAngle + 5 ||
                    Vector3.Distance(target.position, position) > sightLength + 1)
                    target = null;
            }

            thisCurTransform.forward =
                Vector3.RotateTowards(thisCurTransform.forward,
                    isRun ? InputDir : target ? targetDir : repoterForward, Time.deltaTime * speed * 2f, 0);

            #endregion
            for (int i = 0; i < skillKeys.Length; i++)
                if (Input.GetKeyDown(skillKeys[i]))
                    combatUI.ClickSkill(i);
            #region Test

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                actives[0].Use();

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                actives[1].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))

            {
                actives[2].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                actives[12].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                actives[3].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                actives[4].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                actives[5].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                actives[6].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                actives[7].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                actives[8].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                actives[9].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                actives[10].Use();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                actives[11].Use();
            }


            #endregion
        }

        protected override void Attacked()
        {
            Vector3 muzzleForward = muzzle.forward;

            effects[0].Play();
            effects[1].Play();
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.revolver,weapon);
            SpawnManager.Instance.Launch(muzzle.position, muzzleForward,
                dmg, 1 + duration * 0.5f, 35 + speed * 2, range * 0.5f, ref projectileInfo);
            impact -= (15 + dmg * 0.2f) * 0.1f * muzzleForward;
        }

        protected internal override bool Hited(Vector3 attacker, float dmg, float penetrate = 0)
        {
            if (!base.Hited(attacker, dmg, penetrate)) return false;
            if (!(hitScreenAlphaValue < 0.8f)) return true;
            hitScreenAlphaValue += dmg * 3 * (1 / characterStat.maxHP);
            hitScreenColor.a = hitScreenAlphaValue;
            hitScreen.color = hitScreenColor;
            return true;
        }
    }
}