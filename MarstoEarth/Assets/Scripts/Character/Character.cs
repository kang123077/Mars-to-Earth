using System;
using UnityEngine;

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
        [SerializeField]protected StatInfo characterStat;
        [SerializeField]private GameObject prefabModel;
        [SerializeField]protected GameObject prefabHpBar;
        [SerializeField] protected Animator anim;
        
        protected bool attacking;
        protected virtual void Start()
        {
            prefabHpBar =Instantiate(prefabHpBar, UIManager.Instance.transform);
        }

        protected void OnDestroy()
        {
            Destroy(prefabHpBar.gameObject);
        }

        protected abstract void Attack();

        protected void Hit(float dmg,float penetrate=0)
        { 
           var def = characterStat.def * (100 - penetrate) * 0.01f;
           dmg= dmg - def<=0?0:dmg - def;
           characterStat.hp -= dmg;
           if (characterStat.hp <= 0)
           {
               Destroy(gameObject);
           }
        }
    }
}