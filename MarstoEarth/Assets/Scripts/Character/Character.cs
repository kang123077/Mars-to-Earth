using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Character
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] protected StatInfo characterStat;
        [SerializeField] protected Animator anim;
        [SerializeField] protected Collider col;
        
        protected Camera mainCam;
        protected UnityEngine.UI.Slider hpBar;
        protected Transform thisCurTransform;
        protected Transform target;
        protected Character targetCharacter;
        protected Collider[] colliders;
        protected float nockBackResist ;
        protected bool dying;
        protected int level;
        private Vector3 impact;

        protected List<Skill.SPC> Buffs;
        public float dmg { get; set; }
        public float atkSpd { get; set; }
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
                    StartCoroutine(Die());
                _hp = value;
            }
        }
        protected static readonly int movingSpeed = Animator.StringToHash("movingSpeed");
        protected static readonly int attacking = Animator.StringToHash("attacking");


        protected virtual void Awake()
        {
            if(!mainCam)
                mainCam= Camera.main;
            thisCurTransform = transform;
            target = null;
            nockBackResist = characterStat.maxHP * 0.1f;
            impact = Vector3.zero;
            dmg = characterStat.dmg;
            atkSpd = characterStat.atkSpd;
            speed = characterStat.speed;
            def = characterStat.def;
            duration = characterStat.duration;
            hp = characterStat.maxHP;
            range = characterStat.range;
            viewAngle = characterStat.viewAngle;
            
            Buffs = new List<Skill.SPC>();
        }

        protected virtual void Start()
        {
            hpBar =Instantiate(ResourceManager.Instance.hpBar, UIManager.Instance.transform);
        }

        protected virtual void Attack()
        {
            if (!target) return;
            target.gameObject.TryGetComponent(out targetCharacter);
            targetCharacter.Hit(thisCurTransform,dmg,0);
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
        }

        protected virtual void BaseUpdate()
        {
            if (impact.magnitude > 0.1f)
            {
                transform.position += impact * Time.deltaTime;
                impact = Vector3.Lerp(impact, Vector3.zero, 3 * Time.deltaTime);
            }
            foreach (Skill.SPC buff in Buffs) 
                buff.Activation(this);
        }
        protected internal virtual void Hit(Transform attacker, float dmg,float penetrate=0)
        {
            if(dying)
                return; 
            Debug.Log("처맞"+attacker+"한테맞음");
            float penetratedDef = def * (100 - penetrate) * 0.01f;
            dmg= dmg - penetratedDef<=0?0:dmg - penetratedDef;
            hp -= dmg;
            
            hpBar.value = hp / characterStat.maxHP;
            Vector3 horizonPosition = thisCurTransform.position;
            Vector3 attackerPosition = attacker.position;
            horizonPosition.y = attackerPosition.y;
            impact += (horizonPosition - attackerPosition).normalized*(dmg*(1/nockBackResist));
            
        }

        public void AddBuff(Skill.SPC buff)
        {
            buff.Apply(this);
            Buffs.Add(buff);
        }

        public void RemoveBuff(Skill.SPC buff)
        {
            buff.Remove(this);
            Buffs.Remove(buff);
        }
    }
}