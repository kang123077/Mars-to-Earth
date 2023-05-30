
using UnityEngine;

namespace Skill
{
    public class GravityBombSkill : Skill
    {
        // readonly Collider[] colliders;


        private Projectile.ProjectileInfo projectileInfo;
        public GravityBombSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.GravityBomb];
            projectileInfo = new Projectile.ProjectileInfo(0,
                ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Grenade].sharedMesh,
                Projectile.Type.Cannon, (point) =>
                {
                    // GameObject gravitySlot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    GameObject gravitySlot = Object.Instantiate(skillInfo.effects[0]).gameObject;
                    gravitySlot.SetActive(false);
                    gravitySlot.transform.position = point;
                    Projectile.GravityEffect gravity = gravitySlot.AddComponent<Projectile.GravityEffect>();
                    gravity.Init(skillInfo.duration + caster.duration * 0.5f, skillInfo.dmg + caster.dmg * 0.5f,
                        skillInfo.range + caster.range * 0.5f, caster.layerMask, enforce);
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
            Debug.Log(caster.target);
            SpawnManager.Instance.Launch(caster.transform.position + Vector3.up, caster.target ?
                    caster.target.position : caster.transform.position + caster.transform.forward * (skillInfo.range + caster.range * 0.5f),
                    0, skillInfo.duration + caster.duration, skillInfo.speed + caster.speed,
                    skillInfo.range + caster.range * 0.5f, ref projectileInfo);

        }
    }
}
