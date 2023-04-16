using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public abstract class Character : MonoBehaviour
    {

        protected static readonly int movingSpeed = Animator.StringToHash("movingSpeed");
        protected static readonly int attacking = Animator.StringToHash("attacking");
        protected static readonly int onTarget = Animator.StringToHash("onTarget");
        public StatInfo characterStat;
        [SerializeField] protected Animator anim;
        [SerializeField] protected Collider col;
        
        protected Camera mainCam;
        protected UnityEngine.UI.Slider hpBar;
        protected Transform thisCurTransform;
        [HideInInspector] public Transform target;
        protected Character targetCharacter;
        protected Collider[] colliders;
        protected float nockBackResist ;
        [HideInInspector] public bool dying;
        public Skill.Skill onSkill { get; set; }
        private float SPCActionWeight;

        protected int level;
        public Vector3 impact { get; set; }
        public float dmg { get; set; }
        public float coolDecrease { get; set; }
        public float speed { get; set; }
        public float def { get; set; }
        public float duration { get; set; }
        
        public float range { get; set; }
        public float viewAngle { get; set; }
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

        protected List<Skill.SPC> Buffs;
        protected Projectile.ProjectileInfo projectileInfo;


        int buffElementIdx;


        protected virtual void Awake()
        {
            if(!mainCam)
                mainCam= Camera.main;
            thisCurTransform = transform;
            target = null;
            nockBackResist = characterStat.maxHP * 0.1f;
            impact = Vector3.zero;
            dmg = characterStat.dmg;
            coolDecrease = characterStat.coolDecrease;
            speed = characterStat.speed;
            def = characterStat.def;
            duration = characterStat.duration;
            hp = characterStat.maxHP;
            range = characterStat.range;
            viewAngle = characterStat.viewAngle;
            onSkill = null;
            
            Buffs = new List<Skill.SPC>();
            layerMask = (1 << 3 | 1 << 6 ) ^ 1 << gameObject.layer;

        }

        protected virtual void Start()
        {
            hpBar =Instantiate(ResourceManager.Instance.hpBar, UIManager.Instance.transform);
            projectileInfo = new Projectile.ProjectileInfo(layerMask,ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet,null);
            
        }

        protected virtual void Attack()
        {
            if (!target) return;
            target.gameObject.TryGetComponent(out targetCharacter);
            targetCharacter.Hit(thisCurTransform.position,dmg,0);            
            
        }
        protected virtual IEnumerator Die()
        {
            dying = true;
            Destroy(hpBar.gameObject);
            Destroy(col);
            
            anim.Play($"Die",2,0);
            anim.SetLayerWeight(2,1);
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
            // InGameManager.Instance.OnMonsterCleared();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual void BaseUpdate()
        {
            if (impact.magnitude > 0.1f)
            {
                transform.position += impact * Time.deltaTime;
                impact = Vector3.Lerp(impact, Vector3.zero, 3 * Time.deltaTime);
            }
            if (dying)
                return;
            if ( onSkill is null && SPCActionWeight > 0)
                anim.SetLayerWeight(2, SPCActionWeight -= Time.deltaTime*4); 

            for(buffElementIdx=0; buffElementIdx < Buffs.Count; buffElementIdx++)
                Buffs[buffElementIdx].Activation(this);
            
        }
        protected internal virtual void Hit(Vector3 attacker, float dmg,float penetrate=0)
        {
            if(dying)
                return; 
            float penetratedDef = def * (100 - penetrate) * 0.01f;
            dmg= dmg - penetratedDef<=0?0:dmg - penetratedDef;
            hp -= dmg;
            hpBar.value = hp / characterStat.maxHP;
            Vector3 horizonPosition = thisCurTransform.position;
            Vector3 attackerPosition = attacker;
            horizonPosition.y = attackerPosition.y;
            impact += (horizonPosition - attackerPosition).normalized*(dmg*(1/nockBackResist));
        }

        public void AddBuff(Skill.SPC buff)
        {
            buff.Apply?.Invoke(this);
            Buffs.Add(buff);//같은 버프가 걸려있는지 체크해야함

        }
        public void RemoveBuff(Skill.SPC buff)
        {
            buff.Remove?.Invoke(this);
            Buffs.Remove(buff);
        }
        public void PlaySkillClip(Skill.Skill skill)
        {
            onSkill = skill;
            if(skill.skillInfo.clipLayer==2)
                anim.SetLayerWeight(skill.skillInfo.clipLayer, SPCActionWeight=1);
            anim.Play(skill.skillInfo.clipName, skill.skillInfo.clipLayer,0);
            //StartCoroutine(ClipBack(anim.GetCurrentAnimatorClipInfo(2)[0].clip.length));
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