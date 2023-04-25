using UnityEngine;

namespace Character
{
    public class M_CR_42 : Monster
    {
        protected void Update()
        {
            if (!BaseUpdate())
                return;
            if (target)
            {
                ai.SetDestination(target.position);
                if (isAttacking)
                    return;
                Vector3 targetPosition = target.position;
                var position = thisCurTransform.position;
                position.y = targetPosition.y;
                thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward,
                    targetPosition - position, 2 * Time.deltaTime, 0);

                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 1.5f)
                {
                    if (!(targetDistance <= range)) return;
                    anim.SetBool(attacking, isAttacking = true);
                }
                else
                {
                    anim.SetBool(onTarget, target = null);
                }
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward,
                        colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if ((angle < 0 ? -angle : angle) < viewAngle ||
                        Vector3.Distance(colliders[0].transform.position, thisCurTransform.position) <
                        sightLength * 0.3f)
                    {
                        anim.SetBool(onTarget, target = colliders[0].transform);
                    }
                }
            }
        }

    }
}