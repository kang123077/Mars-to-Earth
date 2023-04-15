using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Bite : Skill
    {
        public Bite(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            Vector3 point= caster.transform.position;
            Vector3 targetPosition = point + caster.transform.forward * 2 + caster.transform.up * 3;

            caster.target.TryGetComponent(out Character.Character targetCh);

            SPC bite = null;
            SPC targetBite = null;
            targetBite = new SPC(10, (target) =>
            {
                target.transform.position = targetPosition;
                target.hp -= skillInfo.dmg * Time.deltaTime;
                if (caster.dying)
                {
                    target.RemoveBuff(targetBite);
                }
            });
            bite = new SPC(10, (ch) => {

                targetCh.AddBuff(targetBite);
            },(ch) =>
            {
                caster.transform.position=point;
                
                if (ch.onSkill is null)
                {
                    ch.RemoveBuff(bite);
                    targetCh.RemoveBuff(targetBite);
                }
            },null);
            caster.AddBuff(bite);

            
            return true;
        }
        public override void Effect()
        {
        }
    }
}