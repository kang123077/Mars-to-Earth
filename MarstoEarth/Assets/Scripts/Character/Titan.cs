using UnityEngine;
namespace Character
{
    public class Titan : Monster
    {
     
        protected override void Start()
        {
            base.Start();

            ai.stoppingDistance = 0.5f;
            skill = new Skill.Bite(ResourceManager.Instance.skillInfos[(int)SkillName.Bite]);
        }
        

        protected void Update()
        {
            BaseUpdate();
            if (dying)
                return;

            if (target)
            {
                ai.SetDestination(target.position);

                Vector3 targetPosition = target.position;
                var position = thisCurTransform.position;
                position.y = targetPosition.y;
                thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward, targetPosition - position, 6 * Time.deltaTime, 0);
                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 1.5f)
                {
                    if (!(targetDistance <= range)) return;                    
                    anim.SetBool(attacking, isAttacking = true);
                }
                else
                {
                    anim.SetBool(onTarget, target = null);
                }
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if ((angle < 0 ? -angle : angle) < viewAngle || Vector3.Distance(colliders[0].transform.position, thisCurTransform.position) < sightLength * 0.3f)
                    {
                        anim.SetBool(onTarget, target = colliders[0].transform);
                    }
                }
            }
        }
    }
}