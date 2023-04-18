using Character;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Skill
{
    public class BlockSkill : Skill
    {
        private readonly Collider[] colliders = new Collider[1];
        private SPC block;
        private bool parrying;
        private Action<Vector3, float, float> temp;
        public BlockSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            block = new SPC(0, (ch) => {
                temp = ch.Hit;
                ch.Hit = (attacker, dmg, penetrate) =>
                {
                    ch.Hited(attacker, dmg*0.2f, penetrate);
                    
                    if (parrying||Physics.OverlapSphereNonAlloc(attacker, 1, colliders, ch.layerMask) < 1) return;
                    parrying = true;
                    attacker = colliders[0].transform.position;
                    Vector3 transPos = ch.transform.position;
                    attacker.y = transPos.y;
                    ch.SkillEffect();
                    ResourceManager.Instance.skills[(int)SkillName.ParringKick].Use(ch);
                };
            }, (ch) => {
                ch.Hit = temp;
                if (parrying) return;
                ch.SkillEffect();
            });
        }
        protected override bool Activate()
        {
            parrying = false;
            block.Init(skillInfo.duration + caster.duration * 0.1f);
            caster.PlaySkillClip(this);
            caster.AddBuff(block);

            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            
        }
    }
}