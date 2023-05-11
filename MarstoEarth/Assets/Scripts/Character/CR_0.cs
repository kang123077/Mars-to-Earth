using Skill;
using UnityEngine;

namespace Character
{
    public class CR_0 : Monster
    {
        private Skill.Skill block;

        private float blockEleapse;
        //private Skill.SPC parring;
        //public TrailRenderer swordPath;
    
        protected override void Start()
        {
            base.Start();
            //parring = new SPC((ch) => ch.stun = true,
            // (ch) => ch.stun = false, ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);           
            block = new BlockSkill();
            block.Init(this);
            blockEleapse = 8;
        }
        protected override void Attacked()
        {
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.swing, weapon);
            base.Attacked();
        }
        protected internal override bool Hited(Vector3 attacker, float dmg, float penetrate = 0)
        {
            if (blockEleapse > 15)
            {
                anim.Play("Moving", 1);
                isAttacking = false;
                block.Use();
                blockEleapse = 0;
                return false;
            }
            if (!base.Hited(attacker, dmg, penetrate))
                return false;

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
                         isAttacking = true;
                }
                else
                    target = null;
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Mathf.Acos(Vector3.Dot(thisCurTransform.forward, (colliders[0].transform.position - thisCurTransform.position).normalized)) * Mathf.Rad2Deg;
                        
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