using System;
using UnityEngine;

namespace Skill
{
    public class HyperionSkill : Skill
    {       

        public HyperionSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);
            
            return true;
        }

        public override void Effect()
        {
            GameObject hyperionSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hyperionSlot.SetActive(false);
            Projectile.Hyperion hyperion = hyperionSlot.AddComponent<Projectile.Hyperion>();

            hyperionSlot.transform.position = caster.transform.position;
            hyperionSlot.transform.position += new Vector3(0, 20, 0);
            hyperion.Init(caster.layerMask,skillInfo.dmg+caster.dmg*0.5f,skillInfo.range+caster.range*0.5f,
                skillInfo.duration+caster.duration*0.5f,skillInfo.speed+caster.speed*0.5f);
            hyperionSlot.transform.forward= caster.transform.forward;
            hyperionSlot.SetActive(true);

        }
    }
}