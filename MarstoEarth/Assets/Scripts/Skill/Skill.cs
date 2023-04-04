using System;
using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime;

        protected Character.Character caster;
        protected int layerMask;
        protected Skill()
        {
            lastUsedTime = Time.time;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Character caster)
        {
            if (!this.caster) {
                this.caster = caster;
                layerMask=(1 << 3 | 1 << 6) ^ 1 << caster.gameObject.layer;
            }
            if (Time.time >= lastUsedTime + skillInfo.coolDown)
            {
                if (skillInfo.targetType == TargetType.Target)
                    if(!GetTarget())return;
                Activate();
                lastUsedTime = Time.time;
            }
        }
        protected abstract void Activate();
        public abstract void Effect();
        protected abstract bool GetTarget();
    }
}