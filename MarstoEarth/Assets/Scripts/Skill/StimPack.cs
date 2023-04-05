using System;
using UnityEngine;

namespace Skill
{
    public class StimPack : Skill
    {
        public StimPack(SkillInfo skillInfo)
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
                ch.atkSpd += skillInfo.coolDown;
            }, (ch) =>
            {
                ch.speed -= skillInfo.speed;
                ch.atkSpd -= skillInfo.coolDown;
            }));
        }
    }
}
