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
            SpawnManager.Instance.Launch(caster.transform,caster.target? 
                    caster.target.position:caster.transform.position+ caster.transform.forward*caster.range,caster.gameObject.layer,
                skillInfo.dmg+caster.dmg,Projectile.Mesh.Grenade,Projectile.Type.Cannon);
        }
    }
}