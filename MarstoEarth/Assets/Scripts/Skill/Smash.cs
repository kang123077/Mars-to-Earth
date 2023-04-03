using System;
using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {

        public Smash(SkillInfo skillInfo ) : base ()
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(skillInfo.clipName); // 재생할 애니메이션 호출
            Vector3 point = caster.target.position;
            caster.AddBuff(new SPC(1,(ch) =>
            {
                ch.transform.position =
                    Vector3.MoveTowards(ch.transform.position, point, Time.deltaTime * 5);
            }));
        }

        public override void Effect()
        {
            // 일정 범위 내의 적들에게 데미지를 주는 로직 구현
            Collider[] colliders = Physics.OverlapSphere(caster.target.position, skillInfo.range, (1<<3|1<<6)^1<<caster.gameObject.layer);
            foreach (Collider collider in colliders)
            {
                collider.TryGetComponent(out Character.Character enemy);
                enemy.Hit(caster.transform, skillInfo.dmg, 0);
            }
        }
        protected override bool GetTarget()
        {
            return caster.target;
        }
    }
}
