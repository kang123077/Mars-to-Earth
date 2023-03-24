
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace Character
{
    
    public class Monster : Character
    {
        private static readonly Vector3[] FourDirection = 
            {Vector3.forward,Vector3.right,Vector3.back,Vector3.left };

        
        [SerializeField]private NavMeshAgent ai;
        [SerializeField] private float sightLength;
        [SerializeField] private float viewingAngle;
        private Vector3[] patrolPoints;
        private Transform target;
        private bool trackingPermission;
        private Vector3 lastPosition;
        private List<float> positions;
        private float travelDistance;
        private Collider[] colliders;
        private Camera mainCam;
        private int patrolIdx;
        private IEnumerator StuckCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                float curMoveDistance = Vector3.Distance(lastPosition, transform.position);
                travelDistance += curMoveDistance;
                positions.Add(curMoveDistance);
                if (positions.Count >= 20)
                {
                    if (travelDistance < 0.5f)
                    {
                        trackingPermission = false;
                        target = null;
                        int randIdx ;
                        do randIdx = Random.Range(0, 4);
                        while (patrolIdx == randIdx);
                            
                        patrolIdx = randIdx;
                        ai.SetDestination(patrolPoints[patrolIdx]);
                        positions.Clear();
                        travelDistance = 0;
                    
                    }
                    else
                    {
                        trackingPermission = true;
                        travelDistance -= positions[0];
                        positions.RemoveAt(0);
                    
                    }
                }
                lastPosition = transform.position;
            }
           
        }
        
        private void Awake()
        {
            patrolPoints = new Vector3[4];
            for (int i = 0; i < patrolPoints.Length; i++)
                patrolPoints[i]=  transform.position + FourDirection[i] * (sightLength * 2);
              
            positions = new List<float>();
            trackingPermission = true;
            colliders = new Collider[1];
            mainCam= Camera.main;

            lastPosition = transform.position;
            patrolIdx = Random.Range(0, 4);
            ai.SetDestination(patrolPoints[patrolIdx]);
            ai.speed = characterStat.speed;
            
            StartCoroutine(StuckCheck());
        }

        private void Update()
        {
            Transform thisCurTransform = transform;
            prefabHpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position+Vector3.up*1.5f );
            
            if (target)
            {
                if(attacking)
                    return;
                float targetDistance = Vector3.Distance(target.position, thisCurTransform.position);
                if (targetDistance <= sightLength * 1.5f)
                {
                    if (targetDistance<=characterStat.range)
                    {
                        var position = thisCurTransform.position;
                        thisCurTransform.forward = target.position - position;
                        ai.SetDestination(position);
                        //어택함수 호출
                        positions.Clear();
                        travelDistance = 0;
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
                if (size <= 0) return;
                if (Vector3.Dot(thisCurTransform.forward, (colliders[0].transform.position - thisCurTransform.position).normalized) >
                    Mathf.Cos(viewingAngle * Mathf.Deg2Rad))
                {
                    target = colliders[0].transform;
                }
            }
        }
/*
 * 순찰 포지션은 상하좌우 배열로 받는다.
 * 랜덤으로 하나 골라서 이동한다.
 * 
 *
 * 타겟을 찾았다면
 * 타겟이 공격사거리에 들어오지 않았다면 타겟을향해 이동하고.
 * 그렇지 않으면 이동을 멈추고 공격한다.
 * 공격이 끝나면 반복한다.
 *
 * 그렇지않으면
 * 시야거리안에 적이 들어오는지 체크한다. 
 *
 * 공격중인상태가 아닌데 3초동안 이동한거리가 1 미만이거나 도착하면
 * 랜덤으로 하나 골라서 이동한다. 3초간 추적하지않는다.
 * 
             0.1초마다 0.1초전 거리를  플롯 리스트에 담고 
             총거리에 더한다 
             리스트 카운트가 30개가 되면 
              총거리가 기준치이상이면
              맨앞에꺼를 총거리에서 빼고 제거한다.
              그렇지 않으면 
              리스트를 초기화 한다.
              다른방향을 지정한다.
            
 * 반복한다.
 */
        protected override void Attack()
        {
            throw new System.NotImplementedException();
        }

        
    }
}
