using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
 * 스턴 상태일때는 
 */
namespace Character
{
    public abstract class Character : MonoBehaviour
    {

        protected static readonly int animSpeed = Animator.StringToHash("movingSpeed");
        protected static readonly int attacking = Animator.StringToHash("attacking");
        protected static readonly int onTarget = Animator.StringToHash("onTarget");
        public StatInfo characterStat;
        [SerializeField] public Animator anim;
        [SerializeField] protected Collider col;
        
        protected Camera mainCam;
        protected UnityEngine.UI.Slider hpBar;
        protected Transform thisCurTransform;
        [HideInInspector] public Transform target;
        protected Character targetCharacter;
        protected Collider[] colliders;
        private float nockBackResist ;
        
        public Skill.Skill onSkill { get; set; }
        private float SPCActionWeight;
        public Vector3 impact { get; set; }
        public float dmg { get; set; }
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

        private List<Skill.SPC> Buffs;
        protected Projectile.ProjectileInfo projectileInfo;

        public Func<Vector3,float,float,bool> Hit;
        int buffElementIdx;

        public bool stun;
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
        }

        protected virtual void Start()
        {            
            projectileInfo = new Projectile.ProjectileInfo(layerMask,ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet,null);
        }

        protected virtual bool Attack()
        {
            if (!target) return false;
            target.gameObject.TryGetComponent(out targetCharacter);
            targetCharacter.Hit(thisCurTransform.position,dmg,0);
            return true;
        }
        
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


            //InGameManager.Instance.OnMonsterCleared();
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
                Buffs[buffElementIdx].Activation(this);

            if (stun)
                return false;

            SPCActionWeight =
                Mathf.Clamp(
                    SPCActionWeight += Time.deltaTime * (onSkill && onSkill.skillInfo.clipLayer == 2 ? 3 : -2), 0, 1);
            
            anim.SetLayerWeight(2, SPCActionWeight);
            return true;
        }
        protected internal virtual bool Hited(Vector3 attacker, float dmg,float penetrate=0)
        {
            if (immune)
                return false;
            float penetratedDef = def * (100 - penetrate) * 0.01f;
            dmg= dmg - penetratedDef<=0?0:dmg - penetratedDef;
            hp -= dmg;
           
            hpBar.value = hp / characterStat.maxHP;
            Vector3 horizonPosition = thisCurTransform.position;
            attacker.y = horizonPosition.y;
            impact += (horizonPosition - attacker).normalized*(dmg*(1/nockBackResist));
            if (dying)
                return false;
            return true;
        }

        public void AddBuff(Skill.SPC buff)
        {
            buff.Apply?.Invoke(this);
            Buffs.Add(buff);//같은 버프가 걸려있는지 체크해야함

        }
        // ReSharper disable Unity.PerformanceAnalysis
        public void RemoveBuff(Skill.SPC buff)
        {
            buff.Remove?.Invoke(this);
            Buffs.Remove(buff);
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