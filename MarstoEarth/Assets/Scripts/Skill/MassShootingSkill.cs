using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class MassShootingSkill : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        float atkEleapse ;
        float aniEleapse ;
        private SPC massShooting;
        private float speed;
        public MassShootingSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            projectileInfo = new Projectile.ProjectileInfo(0,
                ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet, null);

            massShooting = new SPC(0, null, (ch) =>
            {
                
                Transform ctr = ((Player)ch).muzzle;
                atkEleapse += Time.deltaTime;
                aniEleapse += Time.deltaTime;
                if (aniEleapse > 0.2f)
                {
                    ch.PlaySkillClip(this);
                    aniEleapse -= 0.2f;
                }
                if (atkEleapse > speed)
                {
                    SpawnManager.Instance.Launch(ctr.position, ctr.forward, skillInfo.dmg + ch.dmg * 0.1f, 2,
                        20 + ch.speed * 2, skillInfo.range * 0.3f + ch.range * 0.1f, ref projectileInfo);
                    atkEleapse -= speed;

                    ch.impact -= 0.25f * ctr.forward;
                }
            }, (ch) =>
            {
                ch.SkillEffect();
            },(int)SkillName.MassShooting);

        }
        protected override bool Activate()
        {
            if(((Player)caster).isRun)
                return false;
            caster.PlaySkillClip(this); 
            
            projectileInfo.lm = caster.layerMask;
            massShooting.Init (skillInfo.duration + caster.duration * 0.5f);
            
            speed = 2/(skillInfo.speed + caster.speed * 0.5f);
            caster.AddBuff(massShooting);
            
            return true;
        }
        public override void Effect()
        {
        }
    }
}