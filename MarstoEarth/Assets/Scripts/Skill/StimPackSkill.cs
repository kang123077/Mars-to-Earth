using System;
using UnityEngine;

namespace Skill
{
    public class StimPackSkill : Skill
    {
        private SPC StimPack;
        private ParticleSystem[] effects = new ParticleSystem[1];
        private byte effectsLength;
        public StimPackSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Stimpack]; 
            StimPack = new SPC( (ch) =>
            {
                for (byte i = 0; i < effectsLength; i++)
                    effects[i].Play();
                ch.speed += skillInfo.speed;
            }, (ch) =>
            {
                for (byte i = 0; i < effectsLength; i++)
                    effects[i].Stop();
                ch.speed -= skillInfo.speed;
            },skillInfo.icon);
            effectsLength = (byte)skillInfo.effects.Length;
        }
        public override void Init(Character.Character caster)
        {
            base.Init(caster);

            for (byte i = 0; i < effectsLength; i++)
            {
                effects[i] = UnityEngine.Object.Instantiate(skillInfo.effects[i], caster.transform);
                effects[i].Stop();
            }
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            StimPack.Init(skillInfo.duration + caster.duration * 0.5f);
            
            return true;
        }

        public override void Effect()
        {
            caster.AddBuff(StimPack);
        }
    }
}
