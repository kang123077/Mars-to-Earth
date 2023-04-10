using System;
using UnityEngine;

namespace Skill
{
    public class AegisBarrierSkill : Skill
    {
        public AegisBarrierSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            return true;
        }

        public override void Effect()
        {
            GameObject aegisBarrierSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            aegisBarrierSlot.SetActive(false);

            Projectile.AegisBarrier aegisBarrier = aegisBarrierSlot.AddComponent<Projectile.AegisBarrier>();

            aegisBarrierSlot.transform.position = caster.transform.position - caster.transform.forward*2;
            aegisBarrier.Init(caster.layerMask, 1 + caster.dmg * 0.02f, skillInfo.range + caster.range * 0.5f,
                skillInfo.duration + caster.duration * 0.5f, skillInfo.speed + caster.speed * 0.5f);
            aegisBarrierSlot.transform.forward = caster.transform.forward;

            aegisBarrierSlot.SetActive(true);

        }
    }
}