namespace Skill
{
    public class GrenadeSkill : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        public GrenadeSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            projectileInfo = new Projectile.ProjectileInfo(0,
                ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Grenade].sharedMesh,
                Projectile.Type.Cannon,null);
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
                    skillInfo.dmg + caster.dmg * 0.5f, skillInfo.duration + caster.duration, skillInfo.speed + caster.speed,
                    skillInfo.range + caster.range * 0.5f, ref projectileInfo);
        }
    }
}