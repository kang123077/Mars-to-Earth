using System;
using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime=0;

        protected Character.Character caster;
        protected int layerMask;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Character caster)
        {
            if (caster.onSkill is not null) return;
            if (!this.caster) {
                this.caster = caster;
                layerMask=(1 << 3 | 1 << 6) ^ 1 << caster.gameObject.layer;
            }
            if (Time.time >= lastUsedTime + skillInfo.coolDown)
            {
                Activate();
                lastUsedTime = Time.time;
            }
        }
        protected abstract void Activate();
        public abstract void Effect();
    }
}