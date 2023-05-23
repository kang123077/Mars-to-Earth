using System;
using UnityEngine;

namespace Skill
{
    public class StimPackSkill : Skill
    {
        private SPC StimPack;
        private ParticleSystem[] effects = new ParticleSystem[1];
        private byte effectsLength;
        private float speedTemp;
        public StimPackSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Stimpack]; 
            StimPack = new SPC( (ch) =>
            {
                for (byte i = 0; i < effectsLength; i++)
                    effects[i].Play();
                speedTemp = ch.speed;
                ch.speed += speedTemp*0.1f*skillInfo.speed;
                if (enforce)
                    ch.speed += +0.05f;
                ch.hp -= ch.MaxHp * 0.1f;
            }, (ch) =>
            {
                for (byte i = 0; i < effectsLength; i++)
                    effects[i].Stop();
                ch.speed -= speedTemp*0.1f*skillInfo.speed;
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
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.itemUse,caster.weapon);
            caster.AddBuff(StimPack);
        }
    }
}
