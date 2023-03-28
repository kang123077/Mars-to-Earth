using System;
using UnityEngine;

namespace Character
{
    public class Player : Character
    {
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");

        private static readonly int onTarget = Animator.StringToHash("onTarget");
        private float _EXP;
        public float EXP
        {
            get => _EXP;
            set
            {
                float curValue=value;
                while (curValue>=100) {
                    curValue -= 100;
                    level++;
                };
                _EXP = curValue;
            }
        }
        
        public float optanium { get; set; }

        private Collider[] itemColliders;
        protected override void Awake()
        {
            base.Awake();
            colliders = new Collider[8];
            itemColliders = new Collider[1];
        }
        protected override void Start()
        {
            base.Start();
            hpBar.transform.position = mainCam.WorldToScreenPoint(transform.position + Vector3.up * 2f);
        }

        protected void Update()
        {
            if (dying)
                return;
            Vector3 position = thisCurTransform.position;
            mainCam.transform.position = position + new Vector3(0, 25, -27.5f);
            if (Physics.OverlapSphereNonAlloc(position, 1f, itemColliders, 1 << 7) > 0)
            {
                itemColliders[0].TryGetComponent(out Item.Item getItem);
                getItem.Use(this);
            }
            
            #region AttackMan
            if (Input.GetMouseButtonDown(0))
                anim.SetTrigger(attacking);
            if (!target)
            {
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, range, colliders,
                    1 << 6);
                if (size > 0)
                {
                    float minCoLength = 1000;
                    for (int i = 0; i < size; i++)
                    {
                        float coLeng = Vector3.Distance(colliders[i].transform.position, thisCurTransform.position);
                        if (minCoLength > coLeng)
                        {
                            minCoLength = coLeng;
                            target = colliders[i].transform;
                        }
                    }
                    anim.SetBool(onTarget, target);
                }
            }
            else if (Vector3.Distance(target.position, thisCurTransform.position) > range + .5f)
                anim.SetBool(onTarget, target = null);
            #endregion
            #region MovingMan
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");
            Vector3 inputDir = new Vector3(xInput, 0, zInput);
            if (xInput != 0 || zInput != 0)
            {
                if (xInput != 0 && zInput != 0)
                {
                    float angle = Vector3.SignedAngle(Vector3.right, inputDir, Vector3.up);
                    float radian = angle * Mathf.Deg2Rad;
                    float movingMag = inputDir.x * (1 / Mathf.Cos(radian)) * 0.7f;
                    inputDir.x = Mathf.Cos(radian) * movingMag;
                    inputDir.z = -Mathf.Sin(radian) * movingMag;
                }

                thisCurTransform.position += inputDir * (Time.deltaTime * speed);
                Vector3 horizonPosition = default;
                Vector3 targetPosition = default;
                if (target)
                {
                    horizonPosition = position;
                    targetPosition = target.position;
                    horizonPosition.y = targetPosition.y;
                }
                thisCurTransform.forward =
                        Vector3.RotateTowards(thisCurTransform.forward, target? targetPosition - horizonPosition:
                            inputDir, 4f * Time.deltaTime, 0);
            }

            Vector3 characterDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position + inputDir));
            anim.SetFloat(X, characterDir.x);
            anim.SetFloat(Z, characterDir.z);
            #endregion
            
        }

        protected override void Attack()
        {
            if (!target)
                return;
            base.Attack();
            if (targetCharacter.hp <= 0)
                anim.SetBool(onTarget, target = null);
        }

    }
}
