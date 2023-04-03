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


        public virtual void Use(Character.Character caster, LayerMask layerMask)
        {   
            this.caster = caster;
            this.layerMask = layerMask;
            //Time.time >= lastUsedTime + skillInfo.coolDown

            if (true)
            {
                if (skillInfo.targetType == TargetType.Target)
                {
                    GetTarget();
                    Activate();
                }
                else
                {
                    Activate();
                    GetTarget();
                }
            }
        }
        protected abstract void Activate();
        protected abstract bool GetTarget();
    }
}