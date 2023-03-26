using System;
using UnityEngine;

namespace Character
{
    public class Player : Character
    {
        
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Z = Animator.StringToHash("z");
        

        private float experience;
        

        protected override void Start()
        {
           base.Start();
           colliders = new Collider[8];
           hpBar.transform.position = mainCam.WorldToScreenPoint(transform.position+Vector3.up*2f );
        }

        protected void Update()
        {
            if(dying)
                return; 
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
                    anim.SetBool(onTarget,target);
                }
            }
            else if (Vector3.Distance(target.position, thisCurTransform.position) > characterStat.range + .5f)
                anim.SetBool(onTarget, target = null);
            
            float xInput = Input.GetAxis("Horizontal");
            float zInput= Input.GetAxis("Vertical");
            Vector3 inputDir = new Vector3(xInput, 0, zInput).normalized;
            if (xInput != 0 || zInput != 0)
            {
                thisCurTransform.position += inputDir * (Time.deltaTime * characterStat.speed);
                if (target)
                {
                    Vector3 horizonPosition = thisCurTransform.position;
                    Vector3 position = target.position;
                    horizonPosition.y = position.y;
                    thisCurTransform.forward = position - horizonPosition;
                }else
                    thisCurTransform.forward = inputDir;    
            }
            
            Vector3 characterDir = (thisCurTransform.InverseTransformPoint(thisCurTransform.position+inputDir)).normalized;
            anim.SetFloat(X,characterDir.x);
            anim.SetFloat(Z,characterDir.z);
            
            if (Input.GetMouseButtonDown(0))
                anim.SetTrigger(attacking);

        }

        
        

    }
}
