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
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
            Vector3 dir= ((Player)caster).inputDir.normalized;
            if (dir.magnitude < 0.1f)
                dir = caster.transform.forward;

            caster.transform.forward = dir;
            caster.AddBuff(new SPC(0.8f, (ch) =>
            {
                ch.transform.position += dir * (Time.deltaTime * (skillInfo.skillSpeed+ch.speed));
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