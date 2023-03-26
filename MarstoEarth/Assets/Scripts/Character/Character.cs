using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace Character
{
    [Serializable]
    
    public abstract class Character : MonoBehaviour
    {
        
        [SerializeField] private string characterName;
        [SerializeField] protected internal StatInfo characterStat;
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

        private static readonly int movingSpeed = Animator.StringToHash("movingSpeed");
        protected static readonly int attacking = Animator.StringToHash("attacking");


        protected virtual void Awake()
        {
            if(!mainCam)
                mainCam= Camera.main;
            target = null;
            characterStat = Instantiate(characterStat);
            anim.SetFloat(movingSpeed,1+characterStat.speed*0.3f);
            thisCurTransform = transform;
            nockBackResist = characterStat.maxHP * 0.1f;
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
            
            float def = characterStat.def * (100 - penetrate) * 0.01f;
            dmg= dmg - def<=0?0:dmg - def;
            characterStat.hp -= dmg;
            hpBar.value = characterStat.hp / characterStat.maxHP;
            Vector3 horizonPosition = thisCurTransform.position;
            Vector3 attackerPosition = attacker.position;
            horizonPosition.y = attackerPosition.y;
            
            if (characterStat.hp <= 0)
                StartCoroutine(Die());
            ai.velocity += (horizonPosition - attackerPosition).normalized*(dmg*(1/nockBackResist));
            thisCurTransform.forward =
                Vector3.RotateTowards(thisCurTransform.forward, attackerPosition - horizonPosition, 80, 30);
        }
    }
}