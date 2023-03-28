using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace Character
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] private StatInfo characterStat;
        [SerializeField] protected Animator anim;
        [SerializeField] protected NavMeshAgent ai;
        [SerializeField] protected Collider col;
        
        protected Camera mainCam;
        protected UnityEngine.UI.Slider hpBar;
        protected Transform thisCurTransform;
        protected Transform target;
        protected Character targetCharacter;
        protected Collider[] colliders;
        private float nockBackResist ;
        protected bool dying;
        protected int level;
        public float dmg { get; set; }
        public float atkSpd { get; set; }
        public float speed { get; set; }
        public float def { get; set; }
        public float duration { get; set; }
        
        public float range { get; set; }
        public float viewingAngle { get; set; }
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
            
            dmg = characterStat.dmg;
            atkSpd = characterStat.atkSpd;
            speed = characterStat.speed;
            def = characterStat.def;
            duration = characterStat.duration;
            hp = characterStat.maxHP;
            range = characterStat.range;
            viewingAngle = characterStat.viewAngle;
        }

        protected virtual void Start()
        {
            hpBar =Instantiate(ResourceManager.Instance.hpBar, UIManager.Instance.transform);
        }

        protected virtual void Attack()
        {
            target.gameObject.TryGetComponent(out targetCharacter);
            targetCharacter.Hit(thisCurTransform,characterStat.dmg,0);
        }
        protected virtual IEnumerator Die()
        {
            dying = true;
            Destroy(hpBar.gameObject);
            Destroy(col);
            ai.ResetPath();
            anim.Play($"Die",2,0);
            anim.SetLayerWeight(2,1);
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }

        protected virtual void Hit(Transform attacker, float dmg,float penetrate=0)
        {
            if(dying)
                return; 
            
            float penetratedDef = def * (100 - penetrate) * 0.01f;
            dmg= dmg - penetratedDef<=0?0:dmg - penetratedDef;
            hp -= dmg;
            hpBar.value = hp / characterStat.maxHP;
            Vector3 horizonPosition = thisCurTransform.position;
            Vector3 attackerPosition = attacker.position;
            horizonPosition.y = attackerPosition.y;
            
            ai.velocity += (horizonPosition - attackerPosition).normalized*(dmg*(1/nockBackResist));
            }
    }
}