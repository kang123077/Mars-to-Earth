using System;
using UnityEngine;

namespace Skill
{
    public class HyperionSkill : Skill
    {       

        public HyperionSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Hyperion];
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);
            
            return true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            GameObject hyperionSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hyperionSlot.SetActive(false);
            Projectile.Hyperion hyperion = hyperionSlot.AddComponent<Projectile.Hyperion>();
            hyperionSlot.layer = 8;
            hyperionSlot.transform.position = caster.transform.position+new Vector3(0, 20, 0);
            hyperion.Init(caster.layerMask,skillInfo.dmg+caster.dmg*0.5f,skillInfo.range+caster.range*0.5f,
                skillInfo.duration+caster.duration*0.5f,skillInfo.speed+caster.speed*0.5f,enforce);
            hyperionSlot.transform.forward= caster.transform.forward;
            hyperionSlot.SetActive(true);

        }
    }
}