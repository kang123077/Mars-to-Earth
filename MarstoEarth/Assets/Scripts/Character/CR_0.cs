using System.Linq;
using UnityEngine;

namespace Character
{
    public class CR_0 : Monster
    {
        private Skill.Skill block;

        private float blockEleapse;

        protected override void Start()
        {
            base.Start();

            block = new Skill.BlockSkill(ResourceManager.Instance.skillInfos[(int)SkillName.Block]);
            blockEleapse = 8;
        }

        protected internal override bool Hited(Vector3 attacker, float dmg, float penetrate = 0)
        {
            if (!base.Hited(attacker, dmg, penetrate))
                return false;
            if (blockEleapse > 15)
            {
                anim.Play("Moving", 1);
                anim.SetBool(attacking, isAttacking = false);
                block.Use(this);
                blockEleapse = 0;
                return false;
            }
            
            ai.SetDestination(SpawnManager.Instance.playerTransform.position);
            return true;
        }

        protected void Update()
        {
            if (!BaseUpdate())
                return;

            if (target)
            {
                ai.SetDestination(target.position);
                if (onSkill is not null && onSkill.skillInfo.clipLayer == 2)
                {
                    ai.ResetPath();
                    positions.Clear();
                    travelDistance = 0;
                    return;
                }

                Vector3 targetPosition = target.position;
                var position = thisCurTransform.position;
                position.y = targetPosition.y;
                thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward,
                    targetPosition - position, 3 * Time.deltaTime, 0);
                blockEleapse += Time.deltaTime;

                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 1f)
                {
                    if (targetDistance <= range + 0.5f)
                        anim.SetBool(attacking, isAttacking = true);
                }
                else
                    anim.SetBool(onTarget, target = null);
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
                        ai.speed = speed * 1.4f;
                    }
                }
            }
        }
    }
}