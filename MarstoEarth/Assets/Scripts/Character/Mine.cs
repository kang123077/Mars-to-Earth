using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float range;
    public float dmg;
    public int layerMask;
    
    private Transform thisTransform;
    private Transform target;
    private float maxDist=1000;
    private static float curDist;
    private readonly Collider[] colliders= new Collider[6];

    public void Init(int lm, float dg, float rg, float dr, float sp)
    {
        layerMask = lm;
        dmg = dg;
        range = rg;
        lifeTime = dr;
        speed = sp;
    }
    private void Awake()
    {
        thisTransform = transform;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
        if (target)
        {
            thisTransform.LookAt(target.position);
            thisTransform.position += Time.deltaTime*speed *thisTransform.forward;
            if (Vector3.Distance(thisTransform.position, target.position) < 0.2f)
            {
                int count =Physics.OverlapSphereNonAlloc(thisTransform.position, range * 0.3f, colliders, layerMask);
                for(int i=0; i<count; i++)
                {
                    colliders[i].TryGetComponent(out Character.Character enemy);
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    enemy.Hit(thisTransform.position, dmg, 0);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            int count =Physics.OverlapSphereNonAlloc(thisTransform.position, range, colliders, layerMask);
            for(int i=0; i<count; i++)
            {
                curDist = Vector3.Distance(thisTransform.position, colliders[i].transform.position);
                if ( curDist< maxDist)
                {
                    target = colliders[i].transform;
                    maxDist = curDist;
                }
            }
        }
    }
    
}