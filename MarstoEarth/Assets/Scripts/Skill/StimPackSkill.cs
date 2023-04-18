using System;
using UnityEngine;

namespace Skill
{
    public class StimPackSkill : Skill
    {
        private SPC StimPack;

        private float duration;
        public StimPackSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            StimPack = new SPC(duration, (ch) =>
            {
                ch.speed += skillInfo.speed;
                ch.coolDecrease += skillInfo.dmg;
            }, (ch) =>
            {
                ch.speed -= skillInfo.speed;
                ch.coolDecrease -= skillInfo.dmg;
            });
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            duration = skillInfo.duration + caster.duration * 0.5f;
            
            return true;
        }

        public override void Effect()
        {
            caster.AddBuff(StimPack);
        }
    }
}
