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
            StimPack = new SPC(0, (ch) =>
            {
                ch.speed += skillInfo.speed;
            }, (ch) =>
            {
                ch.speed -= skillInfo.speed;
            },(int)SkillName.Stimpack);
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            StimPack.Init(skillInfo.duration + caster.duration * 0.5f);
            
            return true;
        }

        public override void Effect()
        {
            caster.AddBuff(StimPack);
        }
    }
}
