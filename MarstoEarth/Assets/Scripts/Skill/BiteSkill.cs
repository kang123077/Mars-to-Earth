using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class BiteSkill : Skill
    {
        SPC targetBite;
        private Vector3 casterPoint;
        private Character.Character targetCh;
        public BiteSkill( Transform LH)
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Bite];
            targetBite = new SPC(10,(target)=> target.stun=true, (target) =>
            {
                target.transform.position = LH.position - caster.transform.up * 1.5f;
                target.Hit(casterPoint, skillInfo.dmg * Time.deltaTime, 0);
                if (caster.dying)
                    target.RemoveBuff(targetBite);
            },(target)=>target.stun=false,skillInfo.icon);
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