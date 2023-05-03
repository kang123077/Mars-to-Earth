using UnityEngine;

namespace Skill
{
    public class BiteSkill : Skill
    {
        SPC targetBite;
        private Vector3 casterPoint;
        private Character.Character targetCh;
        private ParticleSystem effect;

        public BiteSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Bite];
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect = Object.Instantiate(skillInfo.effects[0], caster.muzzle);
            targetBite = new SPC((target) => target.stun = true, (target) =>
            {
                if (caster.dying)
                    target.RemoveBuff(targetBite);
                targetBite.Tick((stack) =>
                {
                    effect.Play();
                    target.Hit(casterPoint, skillInfo.dmg * stack, 0);
                });
                
                target.transform.position = caster.muzzle.position + Vector3.down * 1.5f;
            }, (target) => target.stun = false, skillInfo.icon);
        }

        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            casterPoint = caster.transform.position;
            targetBite.Init(10);
            caster.targetCharacter.AddBuff(targetBite);

            return true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            caster.targetCharacter.RemoveBuff(targetBite);
        }
    }
}