using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class SmashSkill : Skill
    {
        private readonly Collider[] colliders= new Collider[6];
        private SPC smash;
        private float speed;
        private Vector3 dir;
        public SmashSkill(SkillInfo skillInfo )
        {
            this.skillInfo = skillInfo;
            
          
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
            speed = (caster.speed + skillInfo.speed)*0.6f;
            dir =  caster.transform.forward;
            smash = new SPC(10, (ch) =>
            {
                ch.transform.position += dir * (Time.deltaTime*speed);
                if (ch.onSkill is null)
                    ch.RemoveBuff(smash);
            });
            caster.AddBuff(smash);
            
            return true;
        }

        public override void Effect()
        {   
            int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range+caster.range*0.5f, colliders, caster.layerMask);
            for(int i=0; i<count; i++)
            {
                colliders[i].TryGetComponent(out Character.Character enemy);
                enemy?.Hit(caster.transform.position, skillInfo.dmg+caster.dmg*0.5f, 0);
            }
        }
    }
}
