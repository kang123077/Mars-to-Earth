
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Character
{
    
    public class Monster : Character
    {
        protected static readonly Vector3[] FourDirection = 
            {Vector3.forward,Vector3.right,Vector3.back,Vector3.left };
        protected Vector3[] patrolPoints;
        protected List<float> positions;
        protected int patrolIdx;
        
        [SerializeField] protected NavMeshAgent ai;
        
        protected Coroutine StuckCheckCoroutine;
        protected bool trackingPermission;
        protected Vector3 lastPosition;
        protected float travelDistance;
        
        [SerializeField] protected float sightLength;
        protected bool isAttacking;
        [SerializeField] protected float optanium;
        [SerializeField] protected float experience;
        protected IEnumerator StuckCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                float curMoveDistance = Vector3.Distance(lastPosition, ((Component)this).transform.position);
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
                lastPosition = ((Component)this).transform.position;
            }
           
        }
        protected override void Awake()
        {
            base.Awake();
            
            patrolPoints = new Vector3[4];
            for (int i = 0; i < patrolPoints.Length; i++)
                patrolPoints[i]=  thisCurTransform.position + FourDirection[i] * (sightLength * 2);
     
            positions = new List<float>();
            trackingPermission = true;
            colliders = new Collider[1];
            lastPosition = thisCurTransform.position;
            patrolIdx = Random.Range(0, 4);
           
            ai.SetDestination(patrolPoints[patrolIdx]);
            
            ai.speed = speed;
            ai.stoppingDistance = range;
            
            StuckCheckCoroutine =StartCoroutine(StuckCheck());
            anim.SetFloat(movingSpeed,1+speed*0.1f);
            
        }

        
        // ReSharper disable Unity.PerformanceAnalysis
        protected override IEnumerator Die()
        {
            StopCoroutine(StuckCheckCoroutine);
            
            ai.ResetPath();
            SpawnManager.DropOptanium(thisCurTransform.position);
            return base.Die();
        }
        protected override void Attack()
        {
            anim.SetBool(attacking,isAttacking = false );
            positions.Clear();
            travelDistance = 0;
            int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, range+1, colliders, 1 << 3);
            if (target&&size > 0)
            {
                float angle = Vector3.SignedAngle(thisCurTransform.forward, target.position - thisCurTransform.position, Vector3.up);
                if((angle < 0 ? -angle : angle) < viewAngle)
                    base.Attack();
            }else
                Debug.Log("회피 이펙트");
            
        }

        protected internal override void Hit(Transform attacker, float dmg,float penetrate=0)
        {
            base.Hit(attacker, dmg, penetrate);
            Vector3 horizonPosition = thisCurTransform.position;
            Vector3 attackerPosition = attacker.position;
            horizonPosition.y = attackerPosition.y;
            ai.velocity += (horizonPosition - attackerPosition).normalized*(dmg*(1/nockBackResist));
            target = SpawnManager.Instance.playerTransform;
        }
    }
}
