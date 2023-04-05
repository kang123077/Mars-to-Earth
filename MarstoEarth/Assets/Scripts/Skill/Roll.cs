using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Roll : Skill
    {

        public Roll(SkillInfo skillInfo) : base()
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
            caster.AddBuff(new SPC((skillInfo.speed + caster.speed * 0.5f)*0.1f, (ch) =>
            {
                ch.transform.position += dir * (Time.deltaTime * (skillInfo.speed+ch.speed*0.5f));
            }));
        }
        public override void Effect()
        {
        }
        protected override bool GetTarget()
        {
            return caster.target;
        }
    }
}