using UnityEngine;

namespace Skill
{
    public class BiteSkill : Skill
    {
        SPC targetBite;
        private Vector3 casterPoint;
        private Character.Character targetCh;
        private byte timeCount;
        private float curEleapse;
        private static float eleapse = 0.2f;
        private ParticleSystem effect;

        public BiteSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Bite];
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect = UnityEngine.Object.Instantiate(skillInfo.effects[0], caster.muzzle);
            targetBite = new SPC(10, (target) => target.stun = true, (target) =>
            {
                if (caster.dying)
                    target.RemoveBuff(targetBite);
                curEleapse += Time.deltaTime;
                timeCount++;
                if (curEleapse > eleapse)
                {
                    effect.Play();
                    target.Hit(casterPoint, skillInfo.dmg * Time.deltaTime * timeCount, 0);
                    curEleapse -= eleapse;
                    timeCount = 0;
                }

                target.transform.position = caster.muzzle.position + Vector3.down * 1.5f;
            }, (target) => target.stun = false, skillInfo.icon);
        }

        protected override bool Activate()
        {
            caster.PlaySkillClip(this);

            casterPoint = caster.transform.position;

            caster.target.TryGetComponent(out targetCh);

            targetCh.AddBuff(targetBite);

            return true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            targetCh.RemoveBuff(targetBite);
        }
    }
}