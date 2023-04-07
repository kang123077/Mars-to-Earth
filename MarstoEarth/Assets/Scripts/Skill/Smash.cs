using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {
        readonly Collider[] colliders;
        public Smash(SkillInfo skillInfo )
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
            SPC smash = null;
            smash = new SPC(10, (ch) =>
            {
                ch.transform.position +=
                    caster.transform.forward * (Time.deltaTime * (skillInfo.speed + ch.speed * 0.5f));
                if (ch.onSkill is null)
                    ch.RemoveBuff(smash);
            });
            caster.AddBuff(smash);
        }

        public override void Effect()
        {   // 일정 범위 내의 적들에게 데미지를 주는 로직 구현
            int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range, colliders, caster.layerMask);
            for(int i=0; i<count; i++)
            {
                colliders[i].TryGetComponent(out Character.Character enemy);
                enemy.Hit(caster.transform.position, skillInfo.dmg+caster.dmg*0.5f, 0);
            }
        }
    }
}
