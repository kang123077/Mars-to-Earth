using Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Character
{
    public class M_Scout:Monster
    {
        private float skillDelay=2;
        private Vector3 hidingDir;
        private float dist;
        private Skill.Skill skill;
        protected override void Start()
        {
            base.Start();
            skill = new Skill.SpiderMineSkill(ResourceManager.Instance.skillInfos[(int)SkillName.SpiderMine]);
        } 
        protected void Update()
        {
            BaseUpdate();
            if(dying)
                return; 
            
            if (target)
            {
                dist= Vector3.Distance(target.position, thisCurTransform.position);
                if (sightLength * .5f > dist)
                {
                    hidingDir = (thisCurTransform.position - target.position).normalized;
                    skillDelay = 3;
                    ai.ResetPath();
                }

                ai.Move(hidingDir * (Time.deltaTime * speed));
                if(sightLength< dist)
                {
                    skillDelay -= Time.deltaTime;
                    if (skillDelay < 0)
                    {
                        if(!skill.Use(this)) return;
                        skillDelay = 5;
                        target = null;
                    }
                }
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if((angle < 0 ? -angle : angle) < viewAngle|| Vector3.Distance(colliders[0].transform.position,thisCurTransform.position)<sightLength*0.5f)
                    {
                        anim.SetBool(onTarget, target = colliders[0].transform);
                        hidingDir = (thisCurTransform.position - target.position).normalized;
                        ai.ResetPath();
                    }
                }
            }
        }
    }
}