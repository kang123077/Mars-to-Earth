using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class SpiderMine : Installation
    {
       
        private float maxDist = 1000;
        private static float curDist;

        private Transform targetTr;
        private void Update()
        {
            BaseUpdate();
            if (targetTr)
            {
                thisTransform.LookAt(targetTr.position);
                thisTransform.position += Time.deltaTime * speed * thisTransform.forward;
                if (Vector3.Distance(thisTransform.position, targetTr.position) < 0.2f)
                {
                    int count = Physics.OverlapSphereNonAlloc(thisTransform.position, range * 0.3f, colliders,
                        layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out target);
                        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                        target.Hit(thisTransform.position, dmg, 0);
                    }

                    Destroy(gameObject);
                }
            }
            else
            {
                int count = Physics.OverlapSphereNonAlloc(thisTransform.position, range, colliders, layerMask);
                for (int i = 0; i < count; i++)
                {
                    curDist = Vector3.Distance(thisTransform.position, colliders[i].transform.position);
                    if (curDist < maxDist)
                    {
                        targetTr = colliders[i].transform;
                        maxDist = curDist;
                    }
                }
            }
        }

    }
}