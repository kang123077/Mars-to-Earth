using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class ChargeSkill : Skill
    {
        public ChargeSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            if (((Player)caster).onCharge) return false;
            caster.PlaySkillClip(this);

            return true;
        }
        public override void Effect()
        {
            ((Player)caster).onCharge = true;
        }

    }
}