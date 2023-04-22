
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;
using Skill;


namespace Character
{
    
    public class Monster : Character
    {
        private static readonly Vector3[] FourDirection = 
            {Vector3.forward,Vector3.right,Vector3.back,Vector3.left };

        private Vector3[] patrolPoints;
        protected List<float> positions;
        private int patrolIdx;
        
        [SerializeField] protected NavMeshAgent ai;
        private Coroutine StuckCheckCoroutine;
        protected bool trackingPermission;
        private Vector3 lastPosition;
        protected float travelDistance;
        
        [SerializeField] protected float sightLength;

        private bool _isAttacking;
        protected bool isAttacking { get { return _isAttacking; } 
            set {
                if (!value && target)
                    ai.speed=speed*1.4f;
                else
                    ai.speed=speed;
                _isAttacking= value;
            } }

        protected Skill.Skill skill;
        public EnemyType enemyType;

        private IEnumerator StuckCheck()
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
                        trackingPermission = true;
                }
                lastPosition = transform.position;
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
            ai.stoppingDistance = range-1;
            StuckCheckCoroutine =StartCoroutine(StuckCheck());
        }
        protected override void Start()
        {
            base.Start();
            hpBar = Instantiate(ResourceManager.Instance.hpBar, UIManager.Instance.UIs[(int)UIType.Combat].transform);
        }

        protected override void BaseUpdate()
        {
            base.BaseUpdate();
            if(dying)return;

            anim.SetFloat($"z",ai.velocity.magnitude*(1/speed));
            
            hpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position+Vector3.up*1.5f );
        }
        // ReSharper disable Unity.PerformanceAnalysis
        protected override IEnumerator Die()
        {
            StopCoroutine(StuckCheckCoroutine);
            ai.ResetPath();
            Vector3 point = thisCurTransform.position;
            point.y = 0;
            SpawnManager.DropOptanium(point);

            SpawnManager.Instance.monsters.Remove(this);
            SpawnManager.Instance.ClearCheck();
            
            return base.Die();
        }
        protected override void Attack()
        {
            anim.SetBool(attacking,isAttacking = false );
            positions.Clear();
            travelDistance = 0;
            int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, range, colliders, 1 << 3);
            if (target&&size > 0)
            {
                float angle = Vector3.SignedAngle(thisCurTransform.forward, target.position - (thisCurTransform.position-thisCurTransform.forward*range), Vector3.up);
                if((angle < 0 ? -angle : angle) < viewAngle-60)
                {
                    if (skill && skill.Use(this)) return;
                    base.Attack();

                }else
                    Debug.Log("회피 이펙트");
            }else
                Debug.Log("회피 이펙트");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected internal override void Hited(Vector3 attacker, float dmg,float penetrate=0)
        {
            
            base.Hited(attacker, dmg, penetrate);
            if (dying) return;
            ai.SetDestination(SpawnManager.Instance.playerTransform.position);
        }
    }
}
