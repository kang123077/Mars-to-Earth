using System;
using UnityEngine;

namespace Skill
{
    public class StimPackSkill : Skill
    {
        public StimPackSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);            
        }

        public override void Effect()
        {
            caster.AddBuff(new SPC(skillInfo.duration+caster.duration*0.5f, (ch) =>
            {
                ch.speed += skillInfo.speed;
                ch.coolDecrease += skillInfo.dmg;
            }, (ch) =>
            {
                ch.speed -= skillInfo.speed;
                ch.coolDecrease -= skillInfo.dmg;
            }));
        }
    }
}
