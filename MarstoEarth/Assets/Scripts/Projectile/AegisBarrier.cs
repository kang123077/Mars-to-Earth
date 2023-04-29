using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Projectile
{
    public class AegisBarrier : Installation
    {
       
        private static readonly Vector3[] ports = new Vector3[8]
        {
            new Vector3(-.875f, .0f, .0f),
            new Vector3(-.625f, .0f, .0f),
            new Vector3(-.375f, .0f, .0f),
            new Vector3(-.125f, .0f, .0f),
            new Vector3(.125f, .0f, .0f),
            new Vector3(.375f, .0f, .0f),
            new Vector3(.625f, .0f, .0f),
            new Vector3(.875f, .0f, .0f),
        };

        private Transform[] curPorts;
        private Vector3 startPoint;
        private Vector3 targetPoint;

        private Projectile pj;
        public new void Init(int lm, float dg, float rg, float dr, float sp)
        {            
            base.Init(lm, dg, rg, dr, sp);
            
            startPoint= thisTransform.position;
            curPorts = new Transform[8];
            for (int i = 0; i < 8; i++)
            {
                GameObject port = GameObject.CreatePrimitive(PrimitiveType.Cube);
                port.layer = 4;
                port.transform.localScale = range * 0.2f*Vector3.one;
                port.transform.position = transform.position + range * ports[i];
                curPorts[i] = port.transform;
                port.transform.SetParent(transform);
            }
            targetPoint = thisTransform.position + thisTransform.forward * range;
        }
       
        private void Update()
        {
            BaseUpdate();
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, targetPoint, speed * Time.deltaTime);
            
            int size = Physics.OverlapBoxNonAlloc(thisTransform.position, new Vector3(range, 20,2), colliders, Quaternion.LookRotation(thisTransform.forward), layerMask);
            for (int i = 0; i < size; i++)
            {
                if (colliders[i].gameObject.layer == 8)
                {                    
                    colliders[i].TryGetComponent(out pj);
                    if (pj.thisInfo[0].lm !=(layerMask^1<<8))
                        Destroy(pj.gameObject);
                }
                else
                {
                    colliders[i].TryGetComponent(out target);
                    target.impact += (target.transform.position - startPoint).normalized*dmg;
                }
            }
        }
    }
}