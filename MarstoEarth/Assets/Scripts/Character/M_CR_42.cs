using System.Linq;
using UnityEngine;
namespace Character
{
    public class M_CR_42:Monster
    {

        [SerializeField] GameObject club;
        [SerializeField] private Transform RightHand;
        protected override void Awake()
        {
            base.Awake();
            club = Instantiate(club);
            //club = Instantiate(club, RightHand,true);
            club.transform.position = RightHand.position;
            club.transform.SetParent(RightHand);
            //club.transform.localPosition += new Vector3(-0.004f, 0, 0);
            //club.transform.localEulerAngles = new Vector3(0,180,-90);
        }
        protected void Update()
        {
            BaseUpdate();
            if(dying)
                return; 
            
            if (target)
            {
                ai.SetDestination(target.position);
                if(isAttacking)
                    return;
                
                Vector3 targetPosition = target.position;
                float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);
                if (targetDistance <= sightLength * 1.5f)
                {
                    if (!(targetDistance <= range)) return;
                    var position = thisCurTransform.position;
                    position.y = targetPosition.y;
                    thisCurTransform.forward = Vector3.RotateTowards(thisCurTransform.forward, targetPosition - position, 6 * Time.deltaTime, 0);
                    anim.SetBool(attacking, isAttacking = true);
                }
                else
                {
                    anim.SetBool(onTarget, target = null);
                    ai.speed = speed;
                }
            }
            else
            {
                if (!trackingPermission) return;
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Vector3.SignedAngle(thisCurTransform.forward, colliders[0].transform.position - thisCurTransform.position, Vector3.up);
                    if((angle < 0 ? -angle : angle) < viewAngle|| Vector3.Distance(colliders[0].transform.position,thisCurTransform.position)<sightLength*0.3f)
                    {
                        anim.SetBool(onTarget, target = colliders[0].transform);
                        ai.speed = speed*1.5f;
                    }
                }
            }
        }
    }
}