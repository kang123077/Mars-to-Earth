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
            lastUsedTime=Time.time;
            skillInfo = ResourceManager.Instance.skillInfos[0];
        }
        public void Use(Character.Character caster, LayerMask layerMask)
        {
            //lastUsedTime = Time.time;
            this.caster = caster;
            this.layerMask = layerMask;
            Debug.Log("use접근");
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
                lastUsedTime = Time.time;
            }
        }
        protected abstract void Activate();
        protected abstract bool GetTarget();
    }
}