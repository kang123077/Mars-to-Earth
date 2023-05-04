using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class RollSkill : Skill
    {
        private SPC roll;
        private Vector3 dir;
        private ParticleSystem effect;
        public RollSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Roll];
            roll = new SPC((ch)=>
            {
                effect.Play();
                ch.immune = true;
            }, (ch) =>
            {

                ch.transform.position += dir * (Time.deltaTime * (skillInfo.speed + ch.speed));
                ch.anim.SetFloat(Character.Character.MotionTime,(roll.duration-roll.currentTime)*(1/roll.duration));
               
            },(ch)=>
            {
                effect.Stop();
                ch.SkillEffect();
            },skillInfo.icon);
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect= UnityEngine.Object.Instantiate(skillInfo.effects[^1], caster.transform);
        }

        protected override bool Activate()
        {            
            dir= ((Player)caster).InputDir.normalized;
            if (dir.magnitude < 0.1f)
            {
                dir = caster.transform.forward;
            }
            roll.Init(skillInfo.duration+caster.duration*0.5f);
            caster.transform.forward = dir;
            caster.PlaySkillClip(this); 
            caster.AddBuff(roll);
            return true;
        }
        public override void Effect()
        {
            caster.immune = false;
            caster.anim.SetFloat(Character.Character.MotionTime,0);
        }
    }
}