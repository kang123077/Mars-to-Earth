using Character;
using System;
using UnityEngine;

namespace Effect
{
    public class BoomerangSkill : Skill
    {
        private Projectile.Boomerang boomerang;
        public BoomerangSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Boomerang];
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

            GameObject boomerangSlot = UnityEngine.Object.Instantiate(skillInfo.effects[0], caster.transform.position,Quaternion.identity).gameObject;
            boomerangSlot.transform.localScale= 0.5f* (skillInfo.range+caster.range*0.5f)*Vector3.one;
            boomerangSlot.SetActive(false);
            boomerang= boomerangSlot.AddComponent<Projectile.Boomerang>();

            var transform = caster.muzzle.transform;
            boomerang.Init(transform,  skillInfo.duration+caster.duration*0.5f,
                skillInfo.range + caster.range * 0.5f,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.speed + caster.speed * 0.5f,
                caster.layerMask);
            
            boomerangSlot.SetActive(true);

        }
    }
}