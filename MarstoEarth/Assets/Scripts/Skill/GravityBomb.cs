using UnityEngine;

namespace Skill
{
    public class GravityBomb : Skill
    {
        // readonly Collider[] colliders;
        public GravityBomb(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;

            projectileInfo.ms = ResourceManager.Instance.projectileMesh[(int)Projectile.Mesh.Grenade].sharedMesh;
            projectileInfo.ty = Projectile.Type.Cannon;
            projectileInfo.ef = (point) =>
            {
                Debug.Log("중력탄");
                GameObject gravitySlot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gravitySlot.SetActive(false);
                gravitySlot.transform.position = point;
                GravityEffect gravity = gravitySlot.AddComponent<GravityEffect>();
                gravity.Init(caster.duration + skillInfo.duration * 0.5f, skillInfo.dmg, skillInfo.range + caster.range * 0.5f, caster.layerMask);
                gravitySlot.SetActive(true);
            };
            
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
        }

        public override void Effect()
        {   // 일정 범위 내의 적들에게 데미지를 주는 로직 구현
            SpawnManager.Instance.Launch(caster.transform.position, caster.target ?
                    caster.target.position : caster.transform.position + caster.transform.forward * caster.range,
                    0, skillInfo.duration + caster.duration, skillInfo.speed + caster.speed,
                    skillInfo.range + caster.range * 0.5f, ref projectileInfo);

        }
    }
}
