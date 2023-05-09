using Character;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Effect
{
    public class MassShootingSkill : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        float atkEleapse ;
        private SPC massShooting;
        private float speed;
        private ParticleSystem[] effects = new ParticleSystem[2];
        
        private byte effectsLength;
        private static readonly int Parring = Animator.StringToHash("parring");

        private bool isShooting;
        
        public MassShootingSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.MassShooting];
            massShooting = new SPC( (ch) =>
            {
                for ( byte i=0; i< effectsLength; i++)
                    effects[i].Play();
                ch.weapon.loop = true;
                ch.weapon.pitch = 1;
                AudioManager.Instance.PlayEffect((int)CombatEffectClip.massShoot,ch.weapon);
            }, (ch) =>
            {
                Transform ctr = ((Player)ch).muzzle;
                atkEleapse += Time.deltaTime;
                
                if (atkEleapse > speed)
                {
                    SpawnManager.Instance.Launch(ctr.position, ctr.forward, skillInfo.dmg + ch.dmg * 0.1f, 2,
                        skillInfo.speed + ch.speed * 2, skillInfo.range * 0.3f + ch.range * 0.1f, ref projectileInfo);
                    atkEleapse -= speed;

                    ch.impact -= 0.25f * ctr.forward;
                }
            }, (ch) =>
            {
                for ( byte i=0; i< effectsLength; i++)
                    effects[i].Stop();
                caster.anim.SetBool(Parring,false);
                isShooting = false;
                ch.weapon.loop = false;
                
                ch.weapon.Stop();
                
                ch.SkillEffect();
            },skillInfo.icon);
            effectsLength = (byte)skillInfo.effects.Length;
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            projectileInfo = new Projectile.ProjectileInfo(caster.layerMask,
                ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet, null);

            effects[0] = Object.Instantiate(skillInfo.effects[0], caster.muzzle);
            effects[0].Stop();
            effects[1] = Object.Instantiate(skillInfo.effects[1], caster.handguard);
            effects[1].Stop();
            
        }
        protected override bool Activate()
        {
          
            if (((Player)caster).isRun)
                return false;
            if (isShooting)
            {
                caster.RemoveBuff(massShooting);
                lastUsedTime -= massShooting.currentTime;
                curCount++;
                return false;
            }
            caster.anim.SetBool(Parring, true);
            caster.PlaySkillClip(this);

            massShooting.Init(skillInfo.duration + caster.duration * 0.5f);

            speed = 4/(skillInfo.speed + caster.speed * 0.5f);
            caster.AddBuff(massShooting);
            isShooting = true;


            return true;           
        }
        public override void Effect()
        {
        }
    }
}