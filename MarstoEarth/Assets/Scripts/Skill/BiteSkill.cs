using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class BiteSkill : Skill
    {
        static SPC targetBite;
        private Vector3 casterPoint;
        private Character.Character targetCh;
        public BiteSkill(SkillInfo skillInfo, Transform LH)
        {
            this.skillInfo = skillInfo;
            targetBite = new SPC(10,(target)=> target.stun=true, (target) =>
            {
                target.transform.position = LH.position - caster.transform.up * 1.5f;
                target.Hit(casterPoint, skillInfo.dmg * Time.deltaTime, 0);
                if (caster.dying)
                    target.RemoveBuff(targetBite);
            },(target)=>target.stun=false,(int)SkillName.Bite);
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            casterPoint = caster.transform.position;

            caster.target.TryGetComponent(out targetCh);

            targetCh.AddBuff(targetBite);
            
            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            targetCh.RemoveBuff(targetBite);
        }
    }
}