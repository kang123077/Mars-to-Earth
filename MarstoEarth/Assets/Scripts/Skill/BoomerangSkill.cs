using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class BoomerangSkill : Skill
    {
        private Projectile.Boomerang boomerang;
        public BoomerangSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            comboCount = 2;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        protected override bool Activate()
        {
            switch (curCount)
            {
                case 1:
                    caster.PlaySkillClip(this);
                    break;
                case 2:
                    if (!boomerang)
                        return false;
                    boomerang.Bomb();
                    break;
            }

            return true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {

            GameObject boomerangSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boomerangSlot.SetActive(false);
            boomerang= boomerangSlot.AddComponent<Projectile.Boomerang>();
            
            boomerang.Init(caster.transform, caster.transform.position+caster.transform.forward*skillInfo.range, skillInfo.duration+caster.duration*0.5f,
                skillInfo.range + caster.range * 0.5f,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.speed + caster.speed * 0.5f,
                caster.layerMask);
            
            boomerangSlot.SetActive(true);

        }
    }
}