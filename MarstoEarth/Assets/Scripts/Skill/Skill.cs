using System;
using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime=0;
        protected Character.Character caster;
        protected Projectile.ProjectileInfo projectileInfo;
        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Character caster)
        {
            if (caster.onSkill is not null) return;
            if (!this.caster) {
                this.caster = caster;
                projectileInfo.lm = caster.layerMask;
            }
            if (Time.time >= lastUsedTime + skillInfo.cool-(skillInfo.cool* 0.01f *caster.coolDecrease))
            {
                Activate();
                lastUsedTime = Time.time;
            }
        }
        protected abstract void Activate();
        public abstract void Effect();
    }
}