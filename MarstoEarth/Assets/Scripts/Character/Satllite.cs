using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satllite : MonoBehaviour
{
    private readonly Collider[] colliders = new Collider[5];
    public float dmg;
    public LayerMask layerMask;
    public float range;
    // Update is called once per frame
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
