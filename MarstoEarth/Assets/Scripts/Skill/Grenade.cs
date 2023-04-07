namespace Skill
{
    public class Grenade : Skill
    {
        private Projectile.ProjectileInfo projectileInfo;
        public Grenade(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;

        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);
            if (projectileInfo.ms is null)
                projectileInfo = new Projectile.ProjectileInfo(caster.layerMask,
                    ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Grenade].sharedMesh,
                    Projectile.Type.Cannon,null);
            
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