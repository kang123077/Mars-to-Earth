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
                ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet, null);
            
            
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this); 
            
            projectileInfo.lm = caster.layerMask;
            speed = 2/(skillInfo.speed + caster.speed * 0.5f);
            massShooting = new SPC(skillInfo.duration+caster.duration*0.5f,null, (ch) =>
            {
                Transform ctr = caster.transform;
                atkEleapse += Time.deltaTime;
                aniEleapse += Time.deltaTime;
                if (aniEleapse > 0.3f)
                {
                    caster.PlaySkillClip(this);
                    aniEleapse -= 0.3f;
                }
                if (atkEleapse > speed)
                {
                    SpawnManager.Instance.Launch(ctr.position,ctr.forward,skillInfo.dmg+caster.dmg*0.1f,2,
                        20+caster.speed*2,skillInfo.range*0.3f+caster.range*0.1f,ref projectileInfo);
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