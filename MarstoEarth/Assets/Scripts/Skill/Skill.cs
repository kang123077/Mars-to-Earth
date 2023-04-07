using System;
using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime=0;
        protected Character.Character caster;
        // ReSharper disable Unity.PerformanceAnalysis
        public static implicit operator bool(Skill obj)
        {
            return obj != null;
        }
        public void Use(Character.Character caster)
        {
            if (caster.onSkill is not null) return;
            if (!this.caster) {
                this.caster = caster;
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