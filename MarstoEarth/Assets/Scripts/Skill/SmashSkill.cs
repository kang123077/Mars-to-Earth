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
        private ParticleSystem effect;
        public SmashSkill( )
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Smash];
            smash = new SPC( (ch) =>
            {
                ch.transform.position += dir * (Time.deltaTime * speed);
            },skillInfo.icon);

        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect= UnityEngine.Object.Instantiate(skillInfo.effects[^1], caster.transform);
        }

        protected override bool Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
            speed = caster.speed + skillInfo.speed*0.5f;
            dir =  caster.transform.forward;
            smash.Init(10);
            caster.AddBuff(smash);
            return true;
        }

        public override void Effect()
        {
            effect.Play();
            caster.RemoveBuff(smash);
            int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range+caster.range*0.5f, colliders, caster.layerMask);
            for(int i=0; i<count; i++)
            {
                colliders[i].TryGetComponent(out Character.Character enemy);
                enemy?.Hit(caster.transform.position, skillInfo.dmg+caster.dmg*0.5f, 0);
            }
            
        }
    }
}
