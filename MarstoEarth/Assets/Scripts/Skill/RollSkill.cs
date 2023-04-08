using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class RollSkill : Skill
    {
        public RollSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); 
            Vector3 dir= ((Player)caster).InputDir.normalized;
            if (dir.magnitude < 0.1f)
                dir = caster.transform.forward;

            caster.transform.forward = dir;
            SPC roll = null;
            roll = new SPC(10, (ch) =>
            {
                ch.transform.position += dir * (Time.deltaTime * (skillInfo.speed + ch.speed));
                if(ch.onSkill is null)
                    ch.RemoveBuff(roll);
            });
            caster.AddBuff(roll);
        }
        public override void Effect()
        {
        }
    }
}