using UnityEngine;

namespace Skill
{
    public abstract class Skill
    {
        protected SkillInfo skillInfo;
        float lastUsedTime;
        protected Character.Character caster;
        protected LayerMask layerMask;

        public void Use(Character.Character caster, LayerMask layerMask)
        {
            this.caster = caster;
            this.layerMask = layerMask;
            if (Time.time >= lastUsedTime + skillInfo.coolDown)
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