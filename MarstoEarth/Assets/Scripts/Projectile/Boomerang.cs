using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Boomerang : MonoBehaviour
    {
        private Transform caster;
        private Vector3 targetPoint;
        private float range;
        private float duration;
        private float damage;
        private float speed;
        private readonly Collider[] colliders = new Collider[8];
        private List<Collider> colliderList = new ();
        private int layerMask;
        private bool isReturn;
        
        private Character.Character enemy; 
        public void Init(Transform ct,Vector3 tp,float dr, float rg, float dmg, float sp, int lm)
        {
            caster = ct;
            targetPoint = tp;
            range = rg;
            damage = dmg;
            duration = dr;
            speed = sp;
            layerMask=lm;
            transform.position = ct.position+ct.forward;
        }

        public void Bomb()
        {
            
            int count = Physics.OverlapSphereNonAlloc(transform.position, range * 0.5f, colliders,
                layerMask);
            for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out enemy);
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                if (!enemy.Hit(transform.position, damage * 1.5f, 0)) continue;
                enemy.AddBuff(new Skill.SPC(duration, (ch) => ch.stun = true, (ch) => ch.stun = false,(int)SkillName.Boomerang));
            }
            Destroy(gameObject);
        }
        void Update()
        {
            if (!caster)
                Destroy(gameObject);
            transform.position = Vector3.MoveTowards(transform.position, isReturn ? caster.position : targetPoint,
                Time.deltaTime * speed);
            
            transform.Rotate(0f, speed * 10 * Time.deltaTime, 0f); // y축 기준 회전
            Vector3 position = transform.position;

            int count = Physics.OverlapSphereNonAlloc(position, range * 0.1f, colliders,
                layerMask);
            for (int i = 0; i < count; i++)
            {
                if (colliderList.Find(el => colliders[i] == el)) continue;
                colliderList.Add(colliders[i]);
                colliders[i].TryGetComponent(out enemy);
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                enemy.Hit(position, damage, 0);
            }
            

            if (position == targetPoint)
            {
                isReturn = true;
                colliderList.Clear();
            }
            else if (position == caster.position)
            {
                Destroy(gameObject);
            }
        }
    }
}