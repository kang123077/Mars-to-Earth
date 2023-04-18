using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class BiteSkill : Skill
    {
        private Transform LH;
        static SPC bite ;
        static SPC targetBite ;
        private Vector3 casterPoint;
        private Character.Character targetCh;
        public BiteSkill(SkillInfo skillInfo, Transform LH)
        {
            this.skillInfo = skillInfo;
            this.LH = LH;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            casterPoint= caster.transform.position;

            caster.target.TryGetComponent(out targetCh);
            targetBite = new SPC(10, (target) =>
            {
                target.transform.position = LH.position-caster.transform.up*1.5f;
                target.Hit(casterPoint,skillInfo.dmg * Time.deltaTime,0);
                if (caster.dying)
                {
                    target.RemoveBuff(targetBite);
                }
            });
            bite = new SPC(10, (ch) => {

                targetCh.AddBuff(targetBite);
            },(ch)=>ch.SkillEffect());
            
            caster.AddBuff(bite);
            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            targetCh.RemoveBuff(targetBite);
        }
    }
}