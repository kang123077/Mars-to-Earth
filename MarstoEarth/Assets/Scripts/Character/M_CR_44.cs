using Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Character
{
    public class M_CR_44:Monster
    {
        [SerializeField] GameObject gun;
        [SerializeField] private Transform RightHand;

        protected override void Awake()
        {
            base.Awake();
            gun = Instantiate(gun, RightHand, true);
            gun.transform.position = RightHand.position;
        }
        protected override void Start()
        {
            base.Start();
            projectileInfo.ms = ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Grenade].sharedMesh;
            projectileInfo.ty = Projectile.Type.Cannon;

        }

        protected override void Attack()
        {
            if(dying)
                return;
            anim.SetBool(attacking, isAttacking = false);
            
            positions.Clear();
            travelDistance = 0;
            SpawnManager.Instance.Launch(thisCurTransform.position, target? target.position: thisCurTransform.forward*range, dmg,2 + duration * 0.5f, 20 + speed * 2, range * 0.3f, ref projectileInfo);
            
        }
        protected void Update()
        {
            BaseUpdate();
            if(dying)
                return;
            
            if (target)
            {
                ai.SetDestination(target.position);
                Vector3 targetPosition = target.position;
                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 2f)
                {
                    if (!(targetDistance <= range)) return;
                    var position = thisCurTransform.position;
                    position.y = targetPosition.y;
                    thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward, targetPosition - position, 6 * Time.deltaTime, 0);
                    anim.SetBool(attacking, isAttacking = true);
                }
                else
                {
                    anim.SetBool(onTarget, target = null);
                    ai.speed = speed;
                }
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if((angle < 0 ? -angle : angle) < viewAngle|| Vector3.Distance(colliders[0].transform.position,thisCurTransform.position)<sightLength*0.3f)
                    {
                        anim.SetBool(onTarget, target = colliders[0].transform);
                        ai.speed = speed * 1.5f;
                    }
                }
            }
        }
    }
}