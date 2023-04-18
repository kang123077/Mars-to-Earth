using System;
using UnityEngine;

namespace Skill
{
    public class DistortionSkill : Skill
    {
        private readonly Collider[] colliders = new Collider[8];
        private Character.Character enemy;
        Vector3 firstPos;
        private int layerMask;
        SPC distortion;
        Vector3 targetPoint;
        public DistortionSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            comboCount = 2;
            distortion = new SPC(1.5f, null, (ch) =>
            {
                ch.transform.position = Vector3.MoveTowards(ch.transform.position, targetPoint, (skillInfo.speed + ch.speed * 0.5f) * Time.deltaTime);
                if (ch.transform.position == targetPoint)
                {
                    ch.RemoveBuff(distortion);
                    ch.SkillEffect();
                }
            }, (ch) =>
            {
                ch.SkillEffect();
            });
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override bool Activate()
        {
            
            switch (curCount)
            {
                case 1:
                    caster.onSkill = this;
                    firstPos=caster.transform.position;
                    targetPoint= caster.target? caster.target.position-caster.target.transform.forward : caster.transform.position+(caster.transform.forward*(skillInfo.range + caster.range * 0.5f));

                   
                    caster.AddBuff(distortion);

                    break;

                case 2:
                    caster.transform.position = firstPos;
                    break;
            }

            return true;
        }

        public override void Effect()
        {
            int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range * 0.2f + caster.range * 0.1f, colliders, caster.layerMask); for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out enemy);
                if(enemy)
                    enemy.Hit(caster.transform.position, skillInfo.dmg + caster.dmg * 0.5f, 0);
            }
        }
    }
}