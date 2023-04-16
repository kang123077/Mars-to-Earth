using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class RollSkill : Skill
    {
        private SPC roll;
        private Vector3 dir;
        public RollSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this); 
            dir= ((Player)caster).InputDir.normalized;
            if (dir.magnitude < 0.1f)
                dir = caster.transform.forward;
            roll = new SPC(10, (ch) =>
            {
                ch.transform.position += dir * (Time.deltaTime * (skillInfo.speed + ch.speed));
                if(ch.onSkill is null)
                    ch.RemoveBuff(roll);
            });
            caster.transform.forward = dir;
            caster.AddBuff(roll);
            
            return true;
        }
        public override void Effect()
        {
        }
    }
}