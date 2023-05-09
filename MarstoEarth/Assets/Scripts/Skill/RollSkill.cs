using Character;
using System;
using UnityEngine;

namespace Effect
{
    public class RollSkill : Skill
    {
        private SPC roll;
        private Vector3 dir;
        private ParticleSystem effect;
        private AudioClip temp;
        public RollSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Roll];
            roll = new SPC((ch)=>
            {
                effect.Play();
                ch.step.Stop();
                temp = ch.step.clip;
                AudioManager.Instance.PlayEffect((int)CombatEffectClip.rolling,ch.step);
                ch.immune = true;
            }, (ch) =>
            {

                ch.transform.position += dir * (Time.deltaTime * (skillInfo.speed + ch.speed));
                ch.anim.SetFloat(Character.Character.MotionTime,(roll.duration-roll.currentTime)*(1/roll.duration));
               
            },(ch)=>
            {
                ch.step.Stop();
                ch.step.clip = temp;
                effect.Stop();
                ch.step.Play();
                ch.SkillEffect();
            },skillInfo.icon);
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect= UnityEngine.Object.Instantiate(skillInfo.effects[^1], caster.transform);
            effect.Stop();
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