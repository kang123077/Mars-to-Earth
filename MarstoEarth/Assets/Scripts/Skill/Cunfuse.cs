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
            caster.PlaySkillClip(this); // ����� �ִϸ��̼� ȣ��
        }

        public override void Effect()
        {   //���� ���뽺ų
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
