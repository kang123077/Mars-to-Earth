using System;
using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        public SkillInfo skillInfo;        
        float lastUsedTime;
        protected Character.Character caster;
        protected LayerMask layerMask;
        protected Skill()
        {
            lastUsedTime = Time.time;
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Character caster, LayerMask layerMask)
        {   
            this.caster = caster;
            this.layerMask = layerMask;
            if (Time.time >= lastUsedTime + skillInfo.coolDown)
            {
                if (skillInfo.targetType == TargetType.Target)
                {
                    if(!GetTarget())return;
                    Activate();
                }
                else
                {
                    Activate();
                    GetTarget();
                }
                lastUsedTime = Time.time;
            }
        }
        protected abstract void Activate();
        public abstract void Effect();
        protected abstract bool GetTarget();
    }
}