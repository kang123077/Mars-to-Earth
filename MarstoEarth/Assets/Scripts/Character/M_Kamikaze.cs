using System;
using UnityEngine;
using System.Collections;
namespace Character
{
    public class M_Kamikaze : Monster
    {
        private bool attackReady;

        protected override void Awake()
        {
            base.Awake();
            ai.stoppingDistance = 0.1f;
        }

        private void OnDestroy()
        {
            if (Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength*0.5f, colliders, 1 << 3) <= 0) return;
            target.gameObject.TryGetComponent(out targetCharacter);
            targetCharacter.Hit(thisCurTransform.position,characterStat.maxHP*0.5f,0);
        }

        public void Roll()
        {
            attackReady = true;
            def += 20;
            ai.speed = speed * 30;
        }
        protected void Update()
        {
            if(dying)
                return; 
            BaseUpdate();
            
            if (target)
            {
                Vector3 targetPosition = target.position;
                if (!isAttacking)
                {
                    anim.SetBool(attacking, isAttacking = true);
                    ai.speed = speed;
                }
                if (attackReady)
                {
                    float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                   
                    ai.SetDestination(target.position);
                    if (targetDistance > sightLength * 2f||targetDistance<=range&&attackReady)
                    {
                        if (targetDistance <= range && attackReady)
                        {
                            target.gameObject.TryGetComponent(out targetCharacter);
                            targetCharacter.Hit(thisCurTransform.position,dmg,0);
                        }
                        anim.SetBool(attacking, isAttacking = false);
                        ai.speed = speed;
                        def -= 20;
                        attackReady = false;
                        target = null;
                    }
                }
                
            }
            else
            {
                
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0) {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if((angle < 0 ? -angle : angle) < viewAngle)
                    {
                        target = colliders[0].transform;
                    }
                }
            }
        }
    }
}
