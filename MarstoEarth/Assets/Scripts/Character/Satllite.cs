using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satllite : MonoBehaviour
{
    private readonly Collider[] colliders = new Collider[5];
    public float dmg;
    public int layerMask;
    public float range;

    public void Init(int lm,float dg, float rg)
    {
        layerMask = lm;
        dmg = dg;
        range = rg;
    }
    
    void Update()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, 0.2f, colliders,
                    layerMask) > 0)
        {
            colliders[0].TryGetComponent(out Character.Character target);
            if (target)
                target.Hit(transform.position, dmg);
        }
    }
}
