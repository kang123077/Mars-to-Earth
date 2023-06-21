using UnityEngine;

namespace Skill
{
    public class SmashSkill : Skill
    {
        private readonly Collider[] colliders = new Collider[6];
        private SPC smash;
        private float speed;
        private Vector3 dir;
        private ParticleSystem effect;
        private float dist;
        public SmashSkill()
        {
            skillInfo = ResourceManager.Instance.OnlyMonsterSkillInfos[(int)OnlyMonsterSkill.Smash];
            //smash = new SPC((ch) => ch.transform.position += dir * (Time.deltaTime * speed), skillInfo.icon);
            smash = new SPC((ch) => {
                float impacting = Time.deltaTime * speed*10 ;
                if ((dist -= impacting*0.5f) > 0)
                {
                    //ch.transform.position += dir * aa;
                    ch.impact += dir * impacting;
                }
                  
                
                }, skillInfo.icon);
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect = UnityEngine.Object.Instantiate(skillInfo.effects[^1], caster.transform);
        }

        protected override bool Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
            speed = caster.speed + skillInfo.speed * 0.5f;
            dir = caster.transform.forward;
            
            dist = caster.target? Vector3.Distance(caster.transform.position, caster.target.transform.position):1000;

            

            smash.Init(10);
            caster.AddBuff(smash);
            
            //caster.impact+= dir * (Time.deltaTime * speed);
            /*
             * 
             * 타겟 위치에 따라 이동량이 달라져야함.
             * 이동량을 정해놓고 
             * 
             */
            return true;
        }

        public override void Effect()
        {
            effect.Play();
            caster.RemoveBuff(smash);
            //caster.impact = Vector3.zero;
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.smash, caster.weapon);
            int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range + caster.range * 0.5f, colliders, caster.layerMask);
            for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out Character.Character enemy);
                enemy?.Hit(caster.transform.position, skillInfo.dmg + caster.dmg * 0.5f, 0);
            }

        }
    }
}
