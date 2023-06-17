using Character;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Skill
{
    public class MassShootingSkill : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        float atkEleapse;
        private SPC massShooting;
        private float speed;
        private ParticleSystem[] effects = new ParticleSystem[2];

        private byte effectsLength;
        private static readonly int Parring = Animator.StringToHash("parring");

        private bool isShooting;
        private Transform ctr;
        public MassShootingSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.MassShooting];
            massShooting = new SPC((ch) =>
            {
                for (byte i = 0; i < effectsLength; i++)
                    effects[i].Play();
                ch.weapon.loop = true;
                ch.weapon.pitch = 1;
                ch.bulletSpeed += 20;
                AudioManager.Instance.PlayEffect((int)CombatEffectClip.massShoot, ch.weapon);
            }, (ch) =>
            {
                atkEleapse += Time.deltaTime;
                if (atkEleapse > speed)
                {
                    SpawnManager.Instance.Launch(ctr.position, ctr.forward, enforce ? skillInfo.dmg + 4 + ch.dmg * 0.3f :
                            skillInfo.dmg + ch.dmg * 0.3f, 2,
                        ch.bulletSpeed, skillInfo.range * 0.3f + ch.range * 0.1f, ref projectileInfo);
                    atkEleapse -= speed;
                    ch.impact -= 0.25f * ctr.forward;
                }
            }, (ch) =>
            {
                for (byte i = 0; i < effectsLength; i++)
                {
                    effects[i].Stop();
                }
                ch.bulletSpeed -= 20;
                caster.anim.SetBool(Parring, false);
                isShooting = false;
                ch.weapon.loop = false;
                ch.weapon.Stop();
                ch.SkillEffect();
            }, skillInfo.icon);
            effectsLength = (byte)skillInfo.effects.Length;
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            ctr = caster.muzzle;
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
                if (enforce)
                {
                    caster.RemoveBuff(massShooting);
                    lastUsedTime -= massShooting.currentTime;
                    curCount++;
                }
                else
                {
                    curCount++;
                    isCombo = curCount <= comboCount;
                }
                return false;
            }
            caster.anim.SetBool(Parring, true);
            caster.PlaySkillClip(this);

            massShooting.Init(skillInfo.duration + caster.duration * 0.5f);

            speed = 4 / (skillInfo.speed + caster.speed * 0.5f);
            caster.AddBuff(massShooting);
            isShooting = true;


            return true;
        }
        public override void Effect()
        {
        }
    }
}