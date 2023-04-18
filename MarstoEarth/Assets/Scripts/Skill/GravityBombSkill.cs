
using UnityEngine;

namespace Skill
{
    public class GravityBombSkill : Skill
    {
        // readonly Collider[] colliders;
        
        private Projectile.ProjectileInfo projectileInfo;
        public GravityBombSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            projectileInfo = new Projectile.ProjectileInfo(0,
                ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Grenade].sharedMesh,
                Projectile.Type.Cannon, (point) =>
                {
                    GameObject gravitySlot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    gravitySlot.SetActive(false);
                    gravitySlot.transform.position = point;
                    Projectile.GravityEffect gravity = gravitySlot.AddComponent<Projectile.GravityEffect>();
                    gravity.Init(skillInfo.duration + caster.duration * 0.5f, skillInfo.dmg,
                        skillInfo.range + caster.range * 0.5f, caster.layerMask);
                    gravitySlot.SetActive(true);
                });
        }

        protected override bool Activate()
        {
            caster.PlaySkillClip(this); 
            projectileInfo.lm = caster.layerMask;
                
            
            return true;
        }


        public override void Effect()
        {  
            SpawnManager.Instance.Launch(caster.transform.position, caster.target ?
                    caster.target.position : caster.transform.position + caster.transform.forward * caster.range,
                    0, skillInfo.duration + caster.duration, skillInfo.speed + caster.speed,
                    skillInfo.range + caster.range * 0.5f, ref projectileInfo);

        }
    }
}
