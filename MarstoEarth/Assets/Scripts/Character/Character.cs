using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace Character
{
    [Serializable]public struct StatInfo
    {
        public float dmg;
        public float atkSpd;
        public float speed;
        public float def;
        public float duration;
        public float hp;
        public float maxHP;
        public float range;
    }

    public abstract class Character : MonoBehaviour
    {
        
        [SerializeField]private string characterName;
        [SerializeField]protected internal StatInfo characterStat;
        [SerializeField] protected Animator anim;
        [SerializeField] protected NavMeshAgent ai;
        [SerializeField] protected Collider col;
        
        protected Camera mainCam;
        protected UnityEngine.UI.Slider hpBar;
        protected Transform thisCurTransform;
        protected Transform target;
        private Character targetCharacter;
        protected Collider[] colliders;
        
        protected bool dying;
        protected uint level;
        
        protected static readonly int movingSpeed = Animator.StringToHash("movingSpeed");
        protected static readonly int onTarget = Animator.StringToHash("onTarget");
        protected static readonly int attacking = Animator.StringToHash("attacking");

        protected virtual void Awake()
        {
            if(!mainCam)
                mainCam= Camera.main;
            target = null;
            anim.SetFloat(movingSpeed,1+characterStat.speed*0.3f);
            thisCurTransform = transform;
        }

        protected virtual void Start()
        {
            hpBar =Instantiate(ResourceManager.Instance.hpBar, UIManager.Instance.transform);
        }

        


        protected virtual void Attack()
        {
            if (!target)
                return;
            if(!targetCharacter)
                target.gameObject.TryGetComponent(out targetCharacter);
            targetCharacter.Hit(characterStat.dmg,0);
            if (targetCharacter.characterStat.hp <= 0)
                anim.SetBool(onTarget, target = null);
            
                
        }
        protected virtual IEnumerator Die()
        {
            dying = true;
            Destroy(hpBar.gameObject);
            Destroy(col);
            Destroy(ai); 
            anim.Play($"Die",2,0);
            anim.SetLayerWeight(2,1);
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
        protected internal void Hit(float dmg,float penetrate=0)
        {
            if(dying)
                return; 
            //ai.SetDestination(SpawnManager.Instance.playerTransform.position);
            float def = characterStat.def * (100 - penetrate) * 0.01f;
            dmg= dmg - def<=0?0:dmg - def;
            characterStat.hp -= dmg;
            hpBar.value = characterStat.hp / characterStat.maxHP;
            if (characterStat.hp <= 0)
                StartCoroutine(Die());
        }
    }
}