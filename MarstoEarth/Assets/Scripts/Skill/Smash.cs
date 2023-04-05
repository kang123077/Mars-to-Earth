using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {
        Collider[] colliders;
        Vector3 point;
        public Smash(SkillInfo skillInfo ) : base ()
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
         
            caster.AddBuff(new SPC((skillInfo.speed + caster.speed * 0.5f)*0.1f, (ch) =>
            {
                ch.transform.position += caster.transform.forward * (Time.deltaTime * (skillInfo.speed + ch.speed * 0.5f));
            }));
        }

        public override void Effect()
        {
            // 일정 범위 내의 적들에게 데미지를 주는 로직 구현
            int size = Physics.OverlapSphereNonAlloc(point, skillInfo.size, colliders, layerMask);
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
            return true;
        }
    }
}
