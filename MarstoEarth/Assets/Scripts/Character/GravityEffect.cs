using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEffect : MonoBehaviour
{
    public float duration;
    public float gravity;
    public float range;
    public int layerMask;

    private readonly Collider[] colliders = new Collider[6];
    private Character.Character target;
    private Vector3 thisTransform;
    public void Init(float dr,float gv,float rg,int lm)
    {
        duration= dr;
        gravity= gv;
        range= rg;
        layerMask= lm;
    }
    private void Awake()
    {
        thisTransform = transform.position;
    }
    void Update()
    {
        duration -= Time.deltaTime;
        int count = Physics.OverlapSphereNonAlloc(thisTransform, range, colliders,
                   layerMask);
        for (int i = 0; i < count; i++)
        {
            colliders[i].TryGetComponent(out target);
            target.impact += gravity * Time.deltaTime * (thisTransform - colliders[i].transform.position).normalized;
        }
        if (duration < 0)
            Destroy(gameObject);
    }
}
