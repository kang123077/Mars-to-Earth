using Character;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Skill
{
    public class BlockSkill : Skill
    {
        private SPC block;
        private SPC parring;
        private bool parrying;
        private Func<Vector3, float, float,bool> temp;
        
        public BlockSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Block];
            block = new SPC(0, (ch) => {
                temp = ch.Hit;
                
                ch.Hit = (attacker, dmg, penetrate) =>
                {
                    if(!ch.Hited(attacker, dmg * 0.2f, penetrate))return false;
                    if (parrying || Physics.OverlapSphereNonAlloc(ch.transform.position, skillInfo.range, caster.colliders, ch.layerMask) < 1) return true;
                    attacker = caster.colliders[0].transform.position;
                    attacker.y = ch.transform.position.y;
                    ch.transform.LookAt(attacker);
                    ch.anim.SetBool($"parring", parrying = true);
                    return true;
                };
            }, (ch) => {
                
                ch.Hit = temp;
                if (parrying) return;
                ch.onSkill = null;
            }, skillInfo.icon);
            parring = new SPC(0, (ch) => ch.stun = true,
                (ch) => ch.stun = false, ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);


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
            Vector3 transPos= caster.transform.position;
            int size = Physics.OverlapSphereNonAlloc(transPos, skillInfo.range + caster.range * 0.2f, caster.colliders, caster.layerMask);
            for(int i =0; i < size; i++)
            {
                caster.colliders[i].TryGetComponent(out caster.targetCharacter);
                if (caster.targetCharacter)
                {
                    if (!caster.targetCharacter.Hit(transPos, skillInfo.dmg + caster.dmg * 0.5f, 0)) return;
                    parring.Init(skillInfo.duration + caster.duration * 0.2f);
                    caster.targetCharacter.AddBuff(parring);
                    caster.targetCharacter.impact -= caster.targetCharacter.transform.forward*3;
                }
            }
        }
    }
}