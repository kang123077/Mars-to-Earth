using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class MassShootingSkill : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        public MassShootingSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this); 
            if (projectileInfo.ms is null)
                projectileInfo = new Projectile.ProjectileInfo(caster.layerMask,
                    ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                    Projectile.Type.Bullet, null);
            
            float atkEleapse = 0;
            float aniEleapse = 0;
            SPC massShooting = null;
            float speed = 2/(skillInfo.speed + caster.speed * 0.5f);
            massShooting = new SPC(skillInfo.duration+caster.duration*0.5f,null, (ch) =>
            {
                atkEleapse += Time.deltaTime;
                aniEleapse += Time.deltaTime;
                if (aniEleapse > 0.3f)
                {
                    caster.PlaySkillClip(this);
                    aniEleapse -= 0.3f;
                }
                if (atkEleapse > speed)
                {
                    SpawnManager.Instance.Launch(caster.transform.position,caster.transform.forward,skillInfo.dmg+caster.dmg*0.1f,2,
                        skillInfo.speed+caster.speed*0.5f,skillInfo.range*0.3f+caster.range*0.1f,ref projectileInfo);
                    atkEleapse -= speed;
                }
            }, (ch) =>
            {
                ch.SkillEffect();
            });
            caster.AddBuff(massShooting);
            
            return true;
        }
        public override void Effect()
        {
        }
    }
}