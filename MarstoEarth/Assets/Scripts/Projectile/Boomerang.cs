using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Boomerang : Installation
    {
        private Transform caster;
        private Vector3 targetPoint;
 
        private List<Collider> colliderList = new ();
    
        private bool isReturn;
        private SPC stun;
        public void Init(Transform ct,float dr, float rg, float dmg, float sp, int lm)
        {
            base.Init(lm, dmg, rg, dr, sp);
            caster = ct;
            transform.position = ct.position;
            Vector3 forward = ct.forward;
            forward.y = 0;
            targetPoint = transform.position + Vector3.up + forward * (range * 3);
            stun = new SPC((ch) => ch.stun = true,
                (ch) => ch.stun = false, ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);
        }

        public void Bomb()
        {
            
            int count = Physics.OverlapSphereNonAlloc(transform.position, range * 0.5f, colliders,
                layerMask);
            for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out target);
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                if (!target.Hit(transform.position, dmg * 1.5f, 0)) continue;
                stun.Init(duration);
                target.AddBuff(stun);                
            }
            SpawnManager.Instance.GetEffect(thisTransform.position,ResourceManager.Instance.skillInfos[(int)SkillName.Boomerang].effects[^1],ResourceManager.Instance.audioClips[(int)AudioClipName.explosion2],range*0.4f);
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
                colliders[i].TryGetComponent(out target);
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                target.Hit(position, dmg, 0);
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