using UnityEngine;

namespace Skill
{
    public class GrenadeSkill : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        public GrenadeSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Grenade];
           
        }
        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            projectileInfo = new Projectile.ProjectileInfo(0,
               ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Grenade].sharedMesh,
               Projectile.Type.Cannon, (point) =>
               {
                   Object.Instantiate(skillInfo.effects[0], point,Quaternion.identity);
                   GameObject fireSlot = Object.Instantiate(skillInfo.effects[1]).gameObject;
                   fireSlot.SetActive(false);
                   fireSlot.transform.position = point;
                   Projectile.Fire fire = fireSlot.AddComponent<Projectile.Fire>();
                   fire.Init(caster.layerMask, (skillInfo.dmg*0.2f + caster.dmg*0.1f), skillInfo.range + caster.range * 0.5f,
                       skillInfo.duration + caster.duration * 0.5f, 0 );
                   fireSlot.SetActive(true);
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
            Debug.Log("target=" + caster.target);
            SpawnManager.Instance.Launch(caster.transform.position, caster.target ?
                    caster.target.position : caster.transform.position + caster.transform.forward * caster.range,
                    skillInfo.dmg + caster.dmg * 0.5f, skillInfo.duration + caster.duration, skillInfo.speed + caster.speed,
                    skillInfo.range + caster.range * 0.5f, ref projectileInfo);
        }
    }
}