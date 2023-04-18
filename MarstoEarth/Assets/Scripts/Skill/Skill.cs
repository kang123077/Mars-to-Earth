using System;
using UnityEngine;

namespace Skill
{
    public struct CurCool
    {
        public bool isCombo;
        public float cool;
    }
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime;
        protected Character.Character caster;
        protected float comboCount = 1;
        protected float curCount = 1;
        private float cool;
        
        public CurCool curCool
        {
            get {
                if (curCount <= comboCount && Time.time < lastUsedTime + cool * (1 / comboCount))
                {
                    return new CurCool
                    {
                        isCombo = true,
                        cool = cool * (1 / comboCount)
                    };
                }
                else
                {
                    return new CurCool
                    {
                        isCombo = false,
                        cool = cool * (curCount - 1) / comboCount
                    };
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static implicit operator bool(Skill obj)
        {
            return obj != null;
        }
        public bool Use(Character.Character caster)
        {
            if (caster.onSkill is not null) return false;
            
            if (!this.caster) {
                this.caster = caster;
            }

            cool = skillInfo.cool - (skillInfo.cool * 0.01f * caster.coolDecrease);
         
            if (curCount<=comboCount&& Time.time<lastUsedTime + cool*(1/comboCount))
            {
                if (!Activate())
                    return false;
                curCount++;
                lastUsedTime = Time.time;
                return true;
            }
            else if (Time.time > lastUsedTime + cool* (curCount-1)/comboCount)
            {
                curCount = 1;
                if (!Activate())
                    return false;
                curCount++;
                lastUsedTime = Time.time;
                return true;
            }

            return false;
        }
        protected abstract bool Activate();
        public abstract void Effect();
    }
}