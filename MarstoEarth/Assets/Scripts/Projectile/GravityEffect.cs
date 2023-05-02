using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class GravityEffect : Installation
    {
        public void Init(float dr, float dg, float rg, int lm)
        {
            base.Init(lm, dr, dg, rg,0);
        }


        void Update()
        {
            BaseUpdate();
            int count = Physics.OverlapSphereNonAlloc(thisTransform.position, range, colliders,
                layerMask);
            for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out target);
                if(target)
                    target.impact += dmg * Time.deltaTime *
                                 (thisTransform.position - colliders[i].transform.position).normalized;
            }

        }
    }
}