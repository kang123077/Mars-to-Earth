using Character;
using UnityEngine;

namespace Skill
{
    public class DistortionSkill : Skill
    {
        //private readonly Collider[] colliders = new Collider[8];
        private Character.Character enemy;
        Vector3 firstPos;
        Vector3 secondPos;
        SPC distortion;
        Vector3 dir;
        // Vector3 targetPoint;
        public override bool enforce
        {
            get=>base.enforce;
            set
            {
                if (value)
                    comboCount = 3;
                base.enforce = value;
            }
        }
        public DistortionSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Distortion];
            comboCount = 2;
            distortion = new SPC(null, (ch) =>
            {
                ch.transform.position += dir;
            }, (ch) => ch.SkillEffect(), skillInfo.icon);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override bool Activate()
        {

            switch (curCount)
            {
                case 1:
                    //distortion.Init(skillInfo.duration+caster.duration*0.5f);
                    caster.onSkill = this;
                    var transform = caster.transform;
                    firstPos = transform.position;
                    dir = ((Player)caster).InputDir.normalized;
                    if (dir.magnitude < 0.1f)
                    {
                        dir = transform.forward;
                    }

                    distortion.Init(0.4f);
                    caster.AddBuff(distortion);
                    //caster.impact += dir * ((skillInfo.range + caster.range * 0.5f) * 7);

                    caster.SkillEffect();
                    break;

                case 2:
                    secondPos = caster.transform.position;
                    caster.transform.position = firstPos;
                    break;
                case 3:
                    caster.transform.position = secondPos;
                    break;
            }

            return true;
        }

        public override void Effect()
        {
            // int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range * 0.2f + caster.range * 0.1f, colliders, caster.layerMask); for (int i = 0; i < count; i++)
            // {
            //     colliders[i].TryGetComponent(out enemy);
            //     if(enemy)
            //         enemy.Hit(caster.transform.position, skillInfo.dmg + caster.dmg * 0.5f, 0);
            // }

        }
    }
}