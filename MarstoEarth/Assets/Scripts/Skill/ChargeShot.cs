using System;
using UnityEngine;

namespace Skill
{
    public class ChargeShot : Skill
    {
        private MonoBehaviour pj;
        private readonly Collider[] colliders= new Collider[6];
        private Projectile.ProjectileInfo projectileInfo;
        private Character.Character target;
        
        public ChargeShot(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);
            if (projectileInfo.ms is null)
                projectileInfo = new Projectile.ProjectileInfo(caster.layerMask,
                    ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                    Projectile.Type.Bullet,  (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(pj.transform.position,
                        skillInfo.range + caster.range * 0.2f, colliders,
                        caster.layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out target);
                        if (target)
                            target.Hit(pj.transform.position, skillInfo.dmg + caster.dmg * 0.5f);
                    }
                });
        }

        public override void Effect()
        {
            Vector3 forward = caster.transform.forward;
            pj= SpawnManager.Instance.Launch(caster.transform.position,forward,0 ,1+caster.duration*0.5f, 25+caster.speed*2,caster.range*0.1f, ref projectileInfo);
            caster.impact -= (skillInfo.dmg + caster.dmg*0.5f)*0.2f*forward;
        }
    }
}