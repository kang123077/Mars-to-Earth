using System;
using UnityEngine;

namespace Effect
{
   
    public abstract class Skill
    {
        public SkillInfo skillInfo;     
        
        protected Character.Character caster;
        protected float comboCount = 1;
        protected float curCount = 1;
        
        public float lastUsedTime;
        private float originCoolTime;
        private bool _isCombo;
        public bool isCombo
        {
            get
            {
                curCoolTime= _isCombo? originCoolTime*(1/comboCount):originCoolTime* (curCount-1)/comboCount;
                return _isCombo;
            }
            set => _isCombo = value;
        }

        public float curCoolTime;
        public static implicit operator bool(Skill obj)
        {
            return obj != null;
        }
        public virtual void Init(Character.Character caster)
        {
            this.caster = caster;
            isCombo = false;
        }
        public bool Use()
        {
            if (caster.onSkill && caster.onSkill!=this) return false;
            
            originCoolTime = skillInfo.cool - (skillInfo.cool*0.01f*caster.speed);
            
            if(!isCombo)
                curCount = 1;
            if (!Activate())
                return false;
            curCount++;
            lastUsedTime = Time.time;
            isCombo = curCount <= comboCount;
            
            return true;
        }
        protected abstract bool Activate();
        public abstract void Effect();
    }
}