using System;
using UnityEngine;

namespace Skill
{
    public class StimPackSkill : Skill
    {
        private SPC StimPack;
        public StimPackSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
          
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);   
            StimPack = new SPC(skillInfo.duration + caster.duration * 0.5f, (ch) =>
            {
                ch.speed += skillInfo.speed;
                ch.coolDecrease += skillInfo.dmg;
            }, (ch) =>
            {
                ch.speed -= skillInfo.speed;
                ch.coolDecrease -= skillInfo.dmg;
            });
            return true;
        }

        public override void Effect()
        {
            caster.AddBuff(StimPack);
        }
    }
}
