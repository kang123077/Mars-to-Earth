
using System.Collections;
using System.Collections.Generic;
using Item;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Character
{
    
    public class Monster : Character
    {
        private static readonly Vector3[] FourDirection = 
            {Vector3.forward,Vector3.right,Vector3.back,Vector3.left };
        private Vector3[] patrolPoints;

        [SerializeField] private float sightLength;
        [SerializeField] private float viewingAngle;
        
        private List<float> positions;
        private Coroutine StuckCheckCoroutine;
        
        private bool trackingPermission;
        private Vector3 lastPosition;
        private float travelDistance;
        private int patrolIdx;
        private bool isAttacking;
        
        private float optanium;
        private float experience;
        private IEnumerator StuckCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                float curMoveDistance = Vector3.Distance(lastPosition, transform.position);
                travelDistance += curMoveDistance;
                positions.Add(curMoveDistance);
                if (positions.Count >= 10)
                {
                    travelDistance -= positions[0];
                    positions.RemoveAt(0);
                    if (travelDistance < 1f)
                    {
                        if (!isAttacking){
                         trackingPermission = false;
                        target = null;
                        
                        Debug.Log("끼어서 타겟제거");
                        int randIdx ;
                        do randIdx = Random.Range(0, 4);
                        while (patrolIdx == randIdx);
                        patrolIdx = randIdx;
                        ai.SetDestination(patrolPoints[patrolIdx]);
                        positions.Clear();
                        travelDistance = 0;
                        }
                    
                    }
                    else
                    {
                        trackingPermission = true;
                    }
                }
                lastPosition = transform.position;
            }
           
        }
        protected override void Awake()
        {
            base.Awake();
            patrolPoints = new Vector3[4];
            for (int i = 0; i < patrolPoints.Length; i++)
                patrolPoints[i]=  transform.position + FourDirection[i] * (sightLength * 2);
            
            positions = new List<float>();
            trackingPermission = true;
            colliders = new Collider[1];
            lastPosition = transform.position;
            patrolIdx = Random.Range(0, 4);
            ai.SetDestination(patrolPoints[patrolIdx]);
            
            ai.speed = speed;
            ai.stoppingDistance = range;
            
            StuckCheckCoroutine =StartCoroutine(StuckCheck());
        }

        protected  void Update()
        {
            if(dying)
                return; 
            anim.SetFloat("z",ai.velocity.magnitude*(1/speed));
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
                if (size > 0) {
                    if (Vector3.Dot(thisCurTransform.forward, (colliders[0].transform.position - thisCurTransform.position).normalized) >
                        Mathf.Cos(viewingAngle * Mathf.Deg2Rad))
                    {
                        target = colliders[0].transform;
                    }
                }
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        protected override IEnumerator Die()
        {
            StopCoroutine(StuckCheckCoroutine);
            SpawnManager.DropOptanium(thisCurTransform.position);
            return base.Die();
        }
        protected override void Attack()
        {
            anim.SetBool(attacking,isAttacking = false );
            
            positions.Clear();
            travelDistance = 0;
            int size = Physics.OverlapSphereNonAlloc(transform.position, range+1, colliders, 1 << 3);
            if (target&&size > 0&&
                Vector3.Dot(transform.forward, (colliders[0].transform.position - transform.position).normalized) >
                Mathf.Cos((viewingAngle-10) * Mathf.Deg2Rad))
            {
                base.Attack();
            }else
                Debug.Log("회피 이펙트");
            
        }

        protected override void Hit(Transform attacker, float dmg,float penetrate=0)
        {
            base.Hit(attacker, dmg, penetrate);
            target = SpawnManager.Instance.playerTransform;
        }

    }
}
