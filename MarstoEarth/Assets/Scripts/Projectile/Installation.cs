using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    protected float speed;
    protected float lifeTime;
    protected float duration;
    protected float range;
    protected float dmg;
    protected int layerMask;

    protected Transform thisTransform;
    protected Character.Character target;

    protected readonly Collider[] colliders = new Collider[6];
    public virtual void Init(int lm, float dg, float rg, float dr, float sp)
    {
        layerMask = lm | 1 << 8;
        dmg = dg;
        range = rg;
        lifeTime = duration= dr;
        speed = sp;
        thisTransform = transform;        
    }

    protected void BaseUpdate()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
    }
}
