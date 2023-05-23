using System;
using UnityEngine;

namespace Skill
{
    public class AegisBarrierSkill : Skill
    {
        public AegisBarrierSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.AegisBarrier];
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            return true;
        }

        public override void Effect()
        {
            GameObject aegisBarrierSlot = new();
            aegisBarrierSlot.SetActive(false);

            
            
            Projectile.AegisBarrier aegisBarrier = aegisBarrierSlot.AddComponent<Projectile.AegisBarrier>();
            
            aegisBarrier.Init(caster.layerMask, 1 + caster.dmg * 0.02f, skillInfo.range + caster.range * 0.5f,
                skillInfo.duration + caster.duration * 0.5f, skillInfo.speed + caster.speed * 0.5f,caster.transform);
            
            aegisBarrierSlot.SetActive(true);
            if (!enforce )return;
            for (int i = 0; i < 4; i++)
            {
                caster.transform.forward = caster.transform.right;
                if(i==3) return;
                GameObject aegisBarrierSlot2 = new();
                aegisBarrierSlot2.SetActive(false);
                Projectile.AegisBarrier aegisBarrier2 = aegisBarrierSlot2.AddComponent<Projectile.AegisBarrier>();
            
                aegisBarrier2.Init(caster.layerMask, 1 + caster.dmg * 0.02f, skillInfo.range + caster.range * 0.5f,
                    skillInfo.duration + caster.duration * 0.5f, skillInfo.speed + caster.speed * 0.5f,caster.transform);
            
                aegisBarrierSlot2.SetActive(true);
            }

        }
    }
}