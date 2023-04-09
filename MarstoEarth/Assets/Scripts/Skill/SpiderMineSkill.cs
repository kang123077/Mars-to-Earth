using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class SpiderMineSkill : Skill
    {
        public SpiderMineSkill(SkillInfo skillInfo)
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

            GameObject spiderMineSlot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            spiderMineSlot.SetActive(false);
            spiderMineSlot.transform.position = caster.transform.position;
            Projectile.SpiderMine mine= spiderMineSlot.AddComponent<Projectile.SpiderMine>();
            mine.Init(caster.layerMask,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.range + caster.range * 0.5f,
                skillInfo.duration+caster.duration*0.5f,skillInfo.speed+caster.speed*0.5f);
     
            spiderMineSlot.SetActive(true);
        }
    }
}