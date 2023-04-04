using System;
using UnityEngine;

namespace Skill
{
    public class Cunfuse : Skill
    {
        public Cunfuse(SkillInfo skillInfo) : base()
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
        }

        public override void Effect()
        {   //몬스터 전용스킬
            caster.target.TryGetComponent(out Character.Character enemy);
            enemy.AddBuff(new SPC(10,
             (ch) => {
                 ch.inputDir *= -1;
                 ch.impact += ch.inputDir.normalized*0.1f;
                 }
             ));
        }
        protected override bool GetTarget()
        {
            return caster.target;
        }
    }
}
