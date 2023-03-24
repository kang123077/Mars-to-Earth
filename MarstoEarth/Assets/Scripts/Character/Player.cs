using System;
using UnityEngine;

namespace Character
{
    public class Player : Character
    {
        private Camera mainCam;
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        private Transform target;
        private Collider[] colliders;

        #region Miles

        private Transform spine;
        private float attackWeight;
        #endregion
        
        protected  void Awake()
        {
            if(!mainCam)
                mainCam= Camera.main;
            target = null;
            colliders = new Collider[8];
            anim.SetFloat("movingSpeed",1+characterStat.speed*0.3f);
            spine= anim.GetBoneTransform(HumanBodyBones.Spine);
            attackWeight = 0;
        }

        protected override void Start()
        {
           base.Start();
           prefabHpBar.transform.position = mainCam.WorldToScreenPoint(transform.position+Vector3.up*2f );
        }
        private void Update()
        {
            Transform thisCurTransform = transform;
            mainCam.transform.position = thisCurTransform.position + new Vector3(0, 25, -27.5f);
            if (!target)
            {
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, characterStat.range, colliders, 1 << 6);
                if (size > 0)
                {
                    float minCoLength = 1000;
                    for (int i =0; i< size ; i++)
                    {
                        float coLeng = Vector3.Distance(colliders[i].transform.position, thisCurTransform.position);
                        if (minCoLength > coLeng)
                        {
                            minCoLength = coLeng;
                            target = colliders[i].transform;
                        }
                    }
                }
            }
            else
                if (Vector3.Distance(target.position, thisCurTransform.position) > characterStat.range+.5f)
                    target = null;
            float xInput = Input.GetAxis("Horizontal");
            float zInput= Input.GetAxis("Vertical");
            Vector3 inputDir = new Vector3(xInput, 0, zInput).normalized;
            if (xInput != 0 || zInput != 0)
            {
                thisCurTransform.position += inputDir * (Time.deltaTime * characterStat.speed);
                thisCurTransform.forward =target? target.position-thisCurTransform.position: inputDir;
            }
            
            Vector3 characterDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position+inputDir)).normalized;
            anim.SetFloat(X,characterDir.x);
            anim.SetFloat(Z,characterDir.z);
            
            if (Input.GetMouseButtonDown(0)&&!attacking)
            {
                attacking = true;
                anim.Play("Shoot",1,.1f);
            }

            if (attacking && attackWeight <= 1)
            {
                
                attackWeight += Time.deltaTime*3;
            }
            else if(!attacking&&attackWeight>=0)
                attackWeight -= Time.deltaTime;
            anim.SetLayerWeight(1,attackWeight);
        }

        protected override void Attack()
        {
            attacking = false;
        }

        private void LateUpdate()
        {
            spine.rotation *= Quaternion.Euler(new Vector3(-45,-15,-10)*attackWeight);
        }
    }
}
