using System;
using UnityEngine;

namespace Skill
{
    public class ChargeShotSkill : Skill
    {        
        private readonly Collider[] colliders= new Collider[6];
        private Projectile.ProjectileInfo projectileInfo;
        private Character.Character target;
        public ChargeShotSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);
            if (projectileInfo.ms is null)
                projectileInfo = new Projectile.ProjectileInfo(caster.layerMask,
                    ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                    Projectile.Type.Bullet,  (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(point,
                        skillInfo.range + caster.range * 0.2f, colliders,
                        caster.layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out target);
                        if (target)
                            target.Hit(point, skillInfo.dmg + caster.dmg * 0.5f);
                    }
                });
            
            return true;
        }

        public override void Effect()
        {
            Vector3 forward = caster.transform.forward;
            SpawnManager.Instance.Launch(caster.transform.position,forward,0 ,1+caster.duration*0.5f, 20+caster.speed*2,skillInfo.range+caster.range*0.5f, ref projectileInfo);
            caster.impact -= (skillInfo.dmg + caster.dmg*0.5f)*0.1f*forward;
        }
    }
}