using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Character
{
    
    public class Monster : Character
    {
        private static readonly Vector3[] trackingDirection = 
            {Vector3.forward,Vector3.right,Vector3.back,Vector3.left };

        private Vector3[] patrolPoints;
        protected List<float> positions;
        private int patrolIdx;
        private float showHpEleapse;
        
        [SerializeField] protected NavMeshAgent ai;
        private Coroutine StuckCheckCoroutine;
        protected bool trackingPermission;
        private Vector3 lastPosition;
        protected float travelDistance;

        private bool _isAttacking;
        protected bool isAttacking { get { return _isAttacking; } 
            set {
                if (!value && target)
                    ai.speed=speed*1.4f;
                else
                    ai.speed=speed;
                _isAttacking= value;
            } }

        public EnemyType enemyType;
        //private NavMeshHit hit;

        private IEnumerator StuckCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                if (stun) continue;
                float curMoveDistance = Vector3.Distance(lastPosition,thisCurTransform.position);
                travelDistance += curMoveDistance;
                positions.Add(curMoveDistance);
                if (positions.Count >= 10)
                {
                    travelDistance -= positions[0];
                    positions.RemoveAt(0);
                    if (travelDistance < 1f)
                    {
                        if (!isAttacking)
                        {
                            trackingPermission = false;
                            target = null;

                            int randIdx;
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
                patrolPoints[i] = thisCurTransform.position + trackingDirection[i] * (sightLength * 2);
            //if (NavMesh.SamplePosition(patrolPoints[i], out hit, sightLength * 2, NavMesh.AllAreas))
            //    patrolPoints[i] = hit.position;                    
            //else
            //    Debug.LogWarning("목적지 위치가 NavMesh 영역 밖에 있습니다.");
            positions = new List<float>();
            trackingPermission = true;
            colliders = new Collider[1];
            lastPosition = thisCurTransform.position;
            patrolIdx = Random.Range(0, 4);
           
            ai.SetDestination(patrolPoints[patrolIdx]);
            ai.stoppingDistance = range-1;
            StuckCheckCoroutine =StartCoroutine(StuckCheck());
            anim.keepAnimatorStateOnDisable=true; // 껏다켜져도 애니메이션이 작동하게 하는 프로퍼티
        }
        protected override void Start()
        {
            base.Start();
            hpBar = Instantiate(ResourceManager.Instance.hpBar, combatUI.transform);
            hpBar.gameObject.SetActive(false);
        }

        protected override bool BaseUpdate()
        {
            hpBar.transform.position = mainCam.WorldToScreenPoint(thisCurTransform.position+Vector3.up*1.5f );
            if (!base.BaseUpdate())
                return false;
            anim.SetFloat($"z",ai.velocity.magnitude*(1/speed));
            
            if (showHpEleapse > 0)
            {
                showHpEleapse -= Time.deltaTime;
                if(showHpEleapse<=0) hpBar.gameObject.SetActive(false);
            }
            return !stun;
        }


        // ReSharper disable Unity.PerformanceAnalysis
        protected override IEnumerator Die()
        {
            StopCoroutine(StuckCheckCoroutine);
            ai.enabled = false;
            anim.SetBool(onTarget, target = null);
            Vector3 point = thisCurTransform.position;
            point.y = 0;
            SpawnManager.DropOptanium(point);

            return base.Die();
        }

        private void OnEnable()
        {
            if(!dying)return;
            target = null;
            hp = characterStat.maxHP;
            ai.enabled = true;

            for (int i = 0; i < patrolPoints.Length; i++)
                patrolPoints[i] = thisCurTransform.position + trackingDirection[i] * (sightLength * 2);

            lastPosition = thisCurTransform.position;
            patrolIdx = Random.Range(0, 4);

            ai.SetDestination(patrolPoints[patrolIdx]);
            hpBar.value = 1;
            hpBar.gameObject.SetActive(false);
            
            col.enabled = true;
            StuckCheckCoroutine =StartCoroutine(StuckCheck());
           
            dying = false;
        }

        protected override bool Attacked()
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
                    return base.Attacked();
                }else
                    Debug.Log("회피 이펙트");
            }else
                Debug.Log("회피 이펙트");
            return false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected internal override bool Hited(Vector3 attacker, float dmg,float penetrate=0)
        {
            if(!base.Hited(attacker, dmg, penetrate))
                return false;
            hpBar.gameObject.SetActive(true);
            showHpEleapse = 4;
            ai.SetDestination(SpawnManager.Instance.playerTransform.position);
            return true;
        }
    }
}
