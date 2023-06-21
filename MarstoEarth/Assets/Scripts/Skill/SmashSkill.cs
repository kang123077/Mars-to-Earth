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
        private SPC stun;
        public SmashSkill()
        {
            skillInfo = ResourceManager.Instance.OnlyMonsterSkillInfos[(int)OnlyMonsterSkill.Smash];
            //smash = new SPC((ch) => ch.transform.position += dir * (Time.deltaTime * speed), skillInfo.icon);
            smash = new SPC((ch) => {
                float impacting = Time.deltaTime * speed*10 ;
                if ((dist -= impacting*0.3f) > 0)
                {
                    //ch.transform.position += dir * aa;
                    ch.impact += dir * impacting;
                }
            }, skillInfo.icon);
           
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect = Object.Instantiate(skillInfo.effects[^1], caster.transform);
        }

        protected override bool Activate()
        {
            caster.PlaySkillClip(this); // 재생할 애니메이션 호출
            speed = caster.speed + skillInfo.speed * 0.5f;
       

            if (caster.target)
            {
                var position = caster.transform.position;
                dir = caster.target.position-position;
                dist = dir.magnitude;
                dir.Normalize();
            }
            else
            {
                dist = 1000;
                dir = caster.transform.forward;
            }
            
            


            smash.Init(10);
            caster.AddBuff(smash);
            
            
            return true;
        }

        public override void Effect()
        {
            effect.Play();
            caster.RemoveBuff(smash);
            //caster.impact = Vector3.zero;
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.crash, caster.weapon);
            int count = Physics.OverlapSphereNonAlloc(caster.transform.position, skillInfo.range + caster.range * 0.5f, colliders, caster.layerMask);
        
            for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out Character.Character enemy);
                // ReSharper disable once Unity.NoNullPropagation
                enemy.Hit(caster.transform.position, skillInfo.dmg + caster.MaxHp * 0.1f, 0);
                if (enforce)
                {
                    stun = new SPC((ch) => ch.stun = true,
                        (ch) => ch.stun = false, ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);
                    stun.Init(2);
                    enemy.AddBuff(stun);
                }

                enemy.impact += (colliders[i].transform.position - caster.transform.position).normalized*2;
            }

        }
    }
}
