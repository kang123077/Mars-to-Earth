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

        public void Roll()
        {
            attackReady = true;
            ai.speed = speed * 20;
        }
        protected void Update()
        {
            if(dying)
                return; 
            anim.SetFloat($"z",ai.velocity.magnitude*(1/speed));
            hpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position+Vector3.up*1.5f );

            
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
                        target.gameObject.TryGetComponent(out targetCharacter);
                        targetCharacter.Hit(thisCurTransform,dmg,0);
                        anim.SetBool(attacking, isAttacking = false);
                        ai.speed = speed;
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
