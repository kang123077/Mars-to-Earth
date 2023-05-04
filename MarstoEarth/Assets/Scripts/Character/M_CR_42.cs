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
                    isAttacking = true;
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, layerMask);
                if (size > 0)
                {
                    float angle =Mathf.Acos(Vector3.Dot(thisCurTransform.forward, (colliders[0].transform.position - thisCurTransform.position).normalized)) * Mathf.Rad2Deg;
                     
                    if ((angle < 0 ? -angle : angle) < viewAngle ||
                        Vector3.Distance(colliders[0].transform.position, thisCurTransform.position) <
                        sightLength * 0.4f)
                    {
                         target = colliders[0].transform;
                    }
                }
            }
        }

    }
}