using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class SpiderMine : Skill
    {
        public SpiderMine(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);
        }

        public override void Effect()
        {

            GameObject spiderMineSlot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            spiderMineSlot.SetActive(false);
            spiderMineSlot.transform.position = caster.transform.position;
            Mine mine= spiderMineSlot.AddComponent<Mine>();
            mine.Init(caster.layerMask,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.range + caster.range * 0.5f,
                skillInfo.duration+caster.duration*0.5f,skillInfo.speed+caster.speed*0.5f);
     
            spiderMineSlot.SetActive(true);
        }
    }
}