namespace Skill
{
    public class Grenade : Skill
    {

        public Grenade(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;

            projectileInfo.ms = ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Grenade].sharedMesh;
            projectileInfo.ty = Projectile.Type.Cannon;
            projectileInfo.ef = null;

        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);
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