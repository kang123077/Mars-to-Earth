using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*
 * 스턴 상태일때는 
 */
namespace Character
{
    public abstract class Character : MonoBehaviour
    {
        public static readonly int MotionTime = Animator.StringToHash("motionTime");
        protected static readonly int animSpeed = Animator.StringToHash("movingSpeed");
        protected static readonly int attacking = Animator.StringToHash("attacking");
        protected static readonly int onTarget = Animator.StringToHash("onTarget");
        public StatInfo characterStat;
        [SerializeField] public Animator anim;
        [SerializeField] protected Collider col;
        
        protected Camera mainCam;
        protected UnityEngine.UI.Slider hpBar;
        protected static CombatUI combatUI;
        protected Transform thisCurTransform;
        [HideInInspector] public Transform target;
        public Character targetCharacter;
        public Collider[] colliders;
        private float nockBackResist ;

        public Transform muzzle;
        public Skill.Skill onSkill;
        private float SPCActionWeight;
        public Vector3 impact;
        public float dmg;
        public float speed;
        public float def;
        public float duration;
        public float range;
        public float viewAngle;
        private float _hp;
        protected internal float hp
        {
            get => _hp;
            set
            {
                if (value > characterStat.maxHP)
                    value = characterStat.maxHP;
                if (value <= 0)
                {
                    SpawnManager.Instance.player.target=null;
                    StartCoroutine(Die());
                }
                _hp = value;
            }
        }
        public int layerMask { get; set; }

        public List<Skill.SPC> Buffs;
        protected Projectile.ProjectileInfo projectileInfo;

        public Func<Vector3,float,float,bool> Hit;
        public Func<bool> Attacken;
        protected int buffElementIdx;
        public bool _stun;
        public bool stun
        {
            get => _stun;
            set
            {
                if (value)
                {
                    anim.SetBool(attacking, false);
                    anim.SetBool(onTarget, false);
                }
                else
                    anim.SetBool(onTarget, target);
                _stun= value;
            }
        }
        public bool immune;

        [HideInInspector] public bool dying;

        protected virtual void Awake()
        {
            if(!mainCam)
                mainCam= Camera.main;
            thisCurTransform = transform;
            target = null;
            nockBackResist = characterStat.maxHP * 0.1f;
            impact = Vector3.zero;
            dmg = characterStat.dmg;
            speed = characterStat.speed;
            def = characterStat.def;
            duration = characterStat.duration;
            hp = characterStat.maxHP;
            range = characterStat.range;
            viewAngle = characterStat.viewAngle;
            onSkill = null;
            
            Buffs = new List<Skill.SPC>();
            layerMask = (1 << 3 | 1 << 6 ) ^ 1 << gameObject.layer;

            anim.SetFloat(animSpeed, 1 + speed * 0.05f);
            
            Hit = Hited;
            Attacken = Attacked;
            
        }

        protected virtual void Start()
        {            
            projectileInfo = new Projectile.ProjectileInfo(layerMask,ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet,null);
            
            combatUI = (CombatUI)UIManager.Instance.UIs[(int)UIType.Combat];
           
        }

        private void Attack()
        {
            //애니메이션 이벤트가 델리게이트를 찾지못하는 이슈 때문에
            //Attack함수를 거쳐 델리게이트를 실행하도록 함
            Attacken();
        }

        protected virtual bool Attacked()
        {
            if (!target) return false;
            target.gameObject.TryGetComponent(out targetCharacter);
            return targetCharacter.Hit(thisCurTransform.position,dmg,0);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual IEnumerator Die()
        {
            dying = true;
            hpBar.gameObject.SetActive(false);
            col.enabled = false;
            Buffs.Clear();
            anim.Play($"Die",2,0);
            anim.SetLayerWeight(2,1);
            yield return new WaitForSeconds(3);
            
            if(this is Monster)
                SpawnManager.Instance.ReleaseMonster((Monster)this);
            else
                Destroy(gameObject);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual bool BaseUpdate()
        {
            if (impact.magnitude > 0.1f)
            {
                transform.position += impact * Time.deltaTime;
                impact = Vector3.Lerp(impact, Vector3.zero, 3 * Time.deltaTime);
            }
            if (dying)
                return false;

            for (buffElementIdx=0; buffElementIdx < Buffs.Count; buffElementIdx++)
            {
                Buffs[buffElementIdx].Activation(this);
            }

            SPCActionWeight =
                Mathf.Clamp(
                    SPCActionWeight += Time.deltaTime * (onSkill && onSkill.skillInfo.clipLayer == 2 ? 3 : -2), 0, 1);
            
            anim.SetLayerWeight(2, SPCActionWeight);
            return true;
        }
        protected internal virtual bool Hited(Vector3 attacker, float dmg,float penetrate=0)
        {
            if (dying)
                return false;
            DamageText dt = combatUI.DMGTextPool.Get();
            dt.transform.position = thisCurTransform.position+new Vector3(0,.8f,0);
            dt.gameObject.SetActive(true);
            if (immune)
            {
                dt.text.text = "Miss";
                return false;
            }
            float penetratedDef = def * (100 - penetrate) * 0.01f;
            dmg= dmg - penetratedDef<=0?0:dmg - penetratedDef;
            hp -= dmg;
            dt.text.text = $"{dmg}";
            hpBar.value = hp / characterStat.maxHP;
                
            Vector3 horizonPosition = thisCurTransform.position;
            attacker.y = horizonPosition.y;
            impact += (horizonPosition - attacker).normalized*(dmg*(1/nockBackResist));
            
            return !dying;
        }

        public virtual bool AddBuff(Skill.SPC buff)
        {
           
            Skill.SPC findBuff = Buffs.Find((el) =>ReferenceEquals(el.icon, buff.icon));
                       
            if (findBuff is null)
            {
                buff.Apply?.Invoke(this);
                Buffs.Add(buff);
            }
            else if (findBuff.currentTime < buff.duration)
                findBuff.Init(buff.duration);               
            

            return findBuff is null;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual int RemoveBuff(Skill.SPC buff)//각각 다른 몬스터들이 준 버프 주소값 
        {           
            buff.Remove?.Invoke(this);
            int findIndex = Buffs.FindIndex((el)=>el==buff);
            Buffs.RemoveAt(findIndex);
            return findIndex;
        }
        public void PlaySkillClip(Skill.Skill skill)
        {
            onSkill = skill;
            anim.Play(skill.skillInfo.clipName, skill.skillInfo.clipLayer,0);            
        }
        public void SkillEffect()
        {
            if (!dying&& onSkill is not null)
            {
                onSkill.Effect();
                onSkill = null;
            }
        }
    }
}