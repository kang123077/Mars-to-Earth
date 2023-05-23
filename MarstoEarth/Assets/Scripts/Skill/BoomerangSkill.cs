using Character;
using System;
using Projectile;
using UnityEngine;

namespace Skill
{
    public class BoomerangSkill : Skill
    {
        private Projectile.Boomerang[] boomerang = new Boomerang[3];
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
                    if (!boomerang[0])
                        return false;
                    boomerang[0].Bomb();
                    if (enforce )
                    {
                        boomerang[1].Bomb();
                        boomerang[2].Bomb();
                    }
                    break;
            }

            return true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            var transform = caster.muzzle.transform;
            if (!enforce)
            {
                
                GameObject boomerangSlot = UnityEngine.Object.Instantiate(skillInfo.effects[0], caster.transform.position,Quaternion.identity).gameObject;
                boomerangSlot.transform.localScale= 0.5f* (skillInfo.range+caster.range*0.5f)*Vector3.one;
                boomerangSlot.SetActive(false);
                boomerang[0]= boomerangSlot.AddComponent<Projectile.Boomerang>();
                boomerang[0].Init(transform,  skillInfo.duration+caster.duration*0.5f,
                    skillInfo.range + caster.range * 0.5f,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.speed + caster.speed * 0.5f,
                    caster.layerMask);
                
                boomerangSlot.SetActive(true);
            }
            else
            {
                caster.transform.Rotate(Vector3.up,-30);
                for (int i = 0; i < 3; i++)
                {
                    
                    GameObject boomerangSlot2 = UnityEngine.Object.Instantiate(skillInfo.effects[0], caster.transform.position,Quaternion.identity).gameObject;
                    boomerangSlot2.transform.localScale= 0.5f* (skillInfo.range+caster.range*0.5f)*Vector3.one;
                    boomerangSlot2.SetActive(false);
                    boomerang[i]= boomerangSlot2.AddComponent<Projectile.Boomerang>();

                    
                    boomerang[i].Init(transform,  skillInfo.duration+caster.duration*0.5f,
                        skillInfo.range + caster.range * 0.5f,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.speed + caster.speed * 0.5f,
                        caster.layerMask);
                    boomerangSlot2.SetActive(true);
                    caster.transform.Rotate(Vector3.up,30);
                    
                }
                caster.transform.Rotate(Vector3.up,-30);
            }

        }
    }
}