
using UnityEngine;

namespace Character
{
    public class PlagueDoctor : Monster
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            SpawnManager.Instance.player.AddBuff(new Skill.SPC(10,0,
                (player) =>((Player)player).inputDir *= -1));
        }

        // Update is called once per frame
        void Update()
        {
            if(dying)
                return; 
            anim.SetFloat($"z",ai.velocity.magnitude*(1/speed));
            hpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position+Vector3.up*1.5f );

            
            if (target)
            {
                if(isAttacking)
                    return;
                Vector3 targetPosition = target.position;
                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 1.5f)
                {
                    if (targetDistance<=range)
                    {
                        var position = thisCurTransform.position;
                        position.y = targetPosition.y;
                        thisCurTransform.forward = targetPosition - position;
                        anim.SetBool(attacking, isAttacking = true);
                    }
                    else
                        ai.SetDestination(target.position);
                }else
                    target = null;
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if((angle < 0 ? -angle : angle) < viewAngle)
                    {
                        target = colliders[0].transform;
                    }
                }
            }
        }
    }
}
