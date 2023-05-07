using Character;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Skill
{
    public class ChargeSkill : Skill
    {
        private Action attackTemp;
        private bool _onCharge;
        private bool onCharge
        {
            get => _onCharge;
            set
            {
                if (value)                
                    effects[2].Play();
                else
                    effects[2].Stop();
                _onCharge = value;
            }
        }
        private ParticleSystem[] effects = new ParticleSystem[3];
        private byte effectsLength;
        private Projectile.ProjectileInfo chargeProjectileInfo;
        public ChargeSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Charge];
            effectsLength = (byte)(skillInfo.effects.Length-1);
        }
        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            
            chargeProjectileInfo = new Projectile.ProjectileInfo(caster.layerMask,
                ResourceManager.Instance.projectileMesh[(int)Projectile.projectileMesh.Bullet1].sharedMesh,
                Projectile.Type.Bullet, (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(point,
                        skillInfo.range + caster.range * 0.2f, caster.colliders,
                        caster.layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        caster.colliders[i].TryGetComponent(out caster.targetCharacter);
                        if (caster.targetCharacter)
                        {
                            if (!caster.targetCharacter.Hit(point, skillInfo.dmg + caster.dmg * 2f, 0)) continue;
                        } 
                    }
                    SpawnManager.Instance.GetEffect(point,skillInfo.effects[^1],ResourceManager.Instance.audioClips[(int)AudioClipName.explosion2], (skillInfo.range + caster.range * 0.2f) * 0.4f);
                });

            
            effects[0]= Object.Instantiate(skillInfo.effects[0], caster.muzzle);            
            effects[1] = Object.Instantiate(skillInfo.effects[1], caster.handguard);
            effects[2] = Object.Instantiate(skillInfo.effects[2], caster.handguard);
            effects[2].Stop();
        }
        protected override bool Activate()
        {
            if (onCharge) return false;
            caster.PlaySkillClip(this);

            return true;
        }
        public override void Effect()
        {
            onCharge = true;
            attackTemp = caster.Attacken;
            caster.Attacken = () =>
            {
                for ( byte i=0; i< effectsLength; i++)
                    effects[i].Play();

                SpawnManager.Instance.Launch(caster.muzzle.position, caster.muzzle.forward, 0, 2,
                    skillInfo.speed + caster.speed * 2,
                    skillInfo.range * 0.5f, ref chargeProjectileInfo);

                caster.impact -= (skillInfo.dmg + caster.dmg * 0.5f) * 0.1f * caster.muzzle.forward;
                caster.Attacken = attackTemp;
                onCharge = false;
            };
        }
    }
}