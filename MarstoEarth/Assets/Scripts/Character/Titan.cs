using System.Collections;
using UnityEngine;

namespace Character
{
    public class Titan : Monster
    {
        private Skill.Skill jumpAttack;
        private float jumpEleapse;
        [SerializeField] private Transform LH;

        private Skill.Skill bite;
        private byte biteEleapse;
        protected override void Start()
        {
            base.Start();

            bite = new Skill.BiteSkill(ResourceManager.Instance.skillInfos[(int)SkillName.Bite], LH);
            jumpAttack = new Skill.SmashSkill(ResourceManager.Instance.skillInfos[(int)SkillName.Smash]);
            jumpEleapse = 8;
        }

        protected override bool Attack()
        {
            if (!base.Attack()) return false;
            biteEleapse++;
            if (biteEleapse <= 10) return true;
            bite.Use(this);
            biteEleapse = 0;
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
                    positions.Clear();
                    travelDistance = 0;
                    return;
                }

                Vector3 targetPosition = target.position;
                var position = thisCurTransform.position;
                position.y = targetPosition.y;
                thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward,
                    targetPosition - position, 3 * Time.deltaTime, 0);
                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength)
                {
                    jumpEleapse += Time.deltaTime;
                    if (targetDistance <= range + 0.5f)
                    {
                        anim.SetBool(attacking, isAttacking = true);
                    }
                    else if (jumpEleapse > 10 && targetDistance > range * 2.5f)
                    {
                        jumpAttack.Use(this);
                        jumpEleapse = 0;
                    }
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
                    }
                }
            }

        }
    }
}