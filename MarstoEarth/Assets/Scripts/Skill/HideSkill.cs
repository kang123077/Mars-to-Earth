﻿using System;
using UnityEngine;

namespace Skill
{
    public class HideSkill : Skill
    {
        public HideSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);     
            
            return true;
        }

        public override void Effect()
        {
            caster.AddBuff(new SPC(skillInfo.duration+caster.duration*0.5f, (ch) =>
            {
                ch.speed += skillInfo.speed;
            }, (ch) =>
            {
                ch.speed -= skillInfo.speed;
            }));
        }
    }
}