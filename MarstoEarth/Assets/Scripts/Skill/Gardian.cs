using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Gardian : Skill
    {
        public Gardian(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);
        }

        public override void Effect()
        {
            Debug.Log("가디언 이펙트");
            SpawnManager.Instance.Launch(caster.transform, caster.transform.forward, caster.gameObject.layer,
           skillInfo.dmg + caster.dmg, Projectile.Mesh.Grenade, Projectile.Type.Satellite);
        }
        
    }
}