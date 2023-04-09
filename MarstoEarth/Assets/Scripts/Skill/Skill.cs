using System;
using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime;
        protected Character.Character caster;
        protected float comboCount = 1;
        protected float curCount = 1;
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

            float cool = skillInfo.cool - (skillInfo.cool * 0.01f * caster.coolDecrease);
         
            if (curCount<=comboCount&& Time.time<lastUsedTime + cool*(1/comboCount))
            {    
                if (!Activate())
                    return;
                curCount++;
                lastUsedTime = Time.time;
            }
            else if (Time.time > lastUsedTime + cool* (curCount-1)/comboCount)
            {
                curCount = 1;
                if (!Activate())
                    return;
                curCount++;
                lastUsedTime = Time.time;
            }
            
        }
        protected abstract bool Activate();
        public abstract void Effect();
    }
}