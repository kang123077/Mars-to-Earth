using System;
using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {
        Collider[] colliders;
        public Smash(SkillInfo skillInfo ) : base ()
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
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
            int size = Physics.OverlapSphereNonAlloc(caster.target.position, skillInfo.size, colliders, layerMask);
            if (size > 0)
            {
                for(int i=0; i<size; i++)
                {
                    colliders[i].TryGetComponent(out Character.Character enemy);
                    enemy.Hit(caster.transform, skillInfo.dmg, 0);
                }
            }
        }
        protected override bool GetTarget()
        {
            return caster.target;
        }
    }
}
