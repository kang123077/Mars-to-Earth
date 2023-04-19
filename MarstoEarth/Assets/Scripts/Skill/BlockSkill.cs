using Character;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Skill
{
    public class BlockSkill : Skill
    {
        private SPC block;
        private bool parrying;
        private Action<Vector3, float, float> temp;
        
        private readonly Collider[] colliders = new Collider[8];
        private Character.Character targetCh;
        public BlockSkill(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
            block = new SPC(0, (ch) => {
                temp = ch.Hit;
                ch.Hit = (attacker, dmg, penetrate) =>
                {
                    ch.Hited(attacker, dmg*0.2f, penetrate);                    
                    if (parrying||Physics.OverlapSphereNonAlloc(ch.transform.position, skillInfo.range, colliders, ch.layerMask) < 1) return;                    
                    attacker = colliders[0].transform.position;
                    attacker.y = ch.transform.position.y;
                    ch.transform.LookAt(attacker);
                    ch.anim.SetBool($"parring", parrying = true);
                };
            }, (ch) => {
                
                ch.Hit = temp;
                if (parrying) return;
                ch.onSkill = null;
            });
        }
        protected override bool Activate()
        {
            caster.anim.SetBool($"parring", parrying = false);            
            block.Init(skillInfo.duration + caster.duration * 0.1f);
            caster.PlaySkillClip(this);
            caster.AddBuff(block);

            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            Debug.Log("참");
            Vector3 transPos= caster.transform.position;
            int size = Physics.OverlapSphereNonAlloc(transPos, skillInfo.range + caster.range * 0.2f, colliders, caster.layerMask);
            for(int i =0; i < size; i++)
            {
                colliders[i].TryGetComponent(out targetCh);
                if (targetCh)
                {
                    targetCh.Hit(transPos, skillInfo.dmg + caster.dmg * 0.5f, 0);
                    targetCh.impact -= targetCh.transform.forward*2;
                }
            }
        }
    }
}