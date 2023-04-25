using Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Character
{
    public class M_CR_44 : Monster
    {
        protected override void Start()
        {
            base.Start();
            projectileInfo.ms = ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Grenade].sharedMesh;
            projectileInfo.ty = Projectile.Type.Cannon;
        }

        protected override bool Attack()
        {
            if (dying)
                return false;
            anim.SetBool(attacking, isAttacking = false);

            positions.Clear();
            travelDistance = 0;
            SpawnManager.Instance.Launch(thisCurTransform.position,
                target ? target.position : thisCurTransform.forward * range, dmg, 2 + duration * 0.5f, 15 + speed * 2,
                range * 0.1f, ref projectileInfo);
            return true;
        }

        protected void Update()
        {
            if (!BaseUpdate())
                return;


            if (target)
            {
                ai.SetDestination(target.position);
                Vector3 targetPosition = target.position;
                var position = thisCurTransform.position;
                position.y = targetPosition.y;
                thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward,
                    targetPosition - position, 2 * Time.deltaTime, 0);

                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 2f)
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
                        anim.SetBool(onTarget, target = colliders[0].transform);
                }
            }

        }
    }
}