using Character;
using System;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Skill
{
    public class ParringKickSkill : Skill
    {
        private readonly Collider[] colliders = new Collider[8];
        private Character.Character targetCh;
        public ParringKickSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            Vector3 transPos= caster.transform.position;
            int size = Physics.OverlapSphereNonAlloc(transPos, skillInfo.range + caster.range * 0.2f, colliders, caster.layerMask);
            for(int i =0; i < size; i++)
            {
                colliders[i].TryGetComponent(out targetCh);
                if (targetCh)
                    targetCh.Hit(transPos, skillInfo.dmg + caster.dmg * 0.5f, 0);
            }

        }
    }
}