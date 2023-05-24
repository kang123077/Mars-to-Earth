using System.Collections;
using UnityEngine;

namespace Character
{
    public class Titan : Monster
    {
        private Skill.Skill jumpAttack;
        private float jumpEleapse;

        public ParticleSystem readySkillEffect;
        private Skill.Skill bite;
        private byte biteEleapse=5;
        protected override void Start()
        {
            base.Start();

            bite = new Skill.BiteSkill();
            bite.Init(this);
            jumpAttack = new Skill.SmashSkill();
            jumpAttack.Init(this);
            jumpEleapse = 8;
        }

        protected override void Attacked()
        {
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.swing, weapon);
            base.Attacked();
            
            biteEleapse++;
            if (biteEleapse > 5)
            {
                readySkillEffect.gameObject.SetActive(true);
            }
            if (biteEleapse <= 6) return;
            int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, range, colliders, 1 << 3);
            if (target && size > 0)
            {
                float angle = Vector3.SignedAngle(thisCurTransform.forward, target.position - (thisCurTransform.position - thisCurTransform.forward * range), Vector3.up);
                if ((angle < 0 ? -angle : angle) < viewAngle - 60)
                {
                    bite.Use();
                }
            }
            readySkillEffect.gameObject.SetActive(false);
            biteEleapse = 0;
        }

        protected void Update()
        {
            if (!BaseUpdate())
                return;
            if (target)
            {
                if (onSkill is not null && onSkill.skillInfo.clipLayer == 2)
                {
                    positions.Clear();
                    travelDistance = 0;
                    return;
                }
                ai.SetDestination(target.position);

                Vector3 targetPosition = target.position;
                var position = thisCurTransform.position;
                position.y = targetPosition.y;
                thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward,
                    targetPosition - position, 3 * Time.deltaTime, 0);
                float targetDistance = Vector3.Distance(targetPosition, position);
                if (targetDistance <= sightLength)
                {
                    jumpEleapse += Time.deltaTime;
                    if (targetDistance <= range + 0.5f)
                    {
                         isAttacking = true;
                    }
                    else if (jumpEleapse > 15 && targetDistance > range * 2.5f)
                    {
                        jumpAttack.Use();
                        jumpEleapse = 0;
                    }
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