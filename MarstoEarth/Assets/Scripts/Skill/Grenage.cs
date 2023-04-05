using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Grenade : Skill
    {
        public Grenade(SkillInfo skillInfo) 
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); 
            
        }

        public override void Effect()
        {
            SpawnManager.Instance.Launch(caster.transform.position,caster.target? 
                    caster.target.position:caster.transform.forward*caster.range,layerMask,
                skillInfo.dmg+caster.dmg,ProjectileType.Bullet1);
        }
    }
}