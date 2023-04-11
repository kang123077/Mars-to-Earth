using UnityEngine;
namespace Character
{
    public class M_CR_42:Monster
    {

        [SerializeField] GameObject club;
        protected override void Start()
        {
            base.Start();
            Transform RightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
            //Debug.Log("rr"+RightHand);
            //Debug.Log("ss"+club);
            //club.transform.position= RightHand.position;
            //club.transform.SetParent(RightHand);
        }
        protected void Update()
        {
            BaseUpdate();
            if(dying)
                return; 
            
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
                    if((angle < 0 ? -angle : angle) < viewAngle|| Vector3.Distance(colliders[0].transform.position,thisCurTransform.position)<sightLength*0.3f)
                    {
                        target = colliders[0].transform;
                    }
                }
            }
        }
    }
}