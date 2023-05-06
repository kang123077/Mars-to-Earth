using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Gardian : Installation
    {
       
        public void Init(int lm, float dg, float rg, float sp)
        {
            base.Init(lm, dg, rg, 0, sp);
           
        }
        private void Awake()
        {
            StartCoroutine(attack(1 / speed));
        }

        IEnumerator attack(float term)
        {
            while (true)
            {
                yield return new WaitForSeconds(term);
                if (Physics.OverlapSphereNonAlloc(transform.position, range, colliders,
                        layerMask) > 0)
                {
                    colliders[0].TryGetComponent(out Character.Character target);
                    if (target)
                        target.Hit(transform.position, dmg,0);
                }
            }
        }
    }
}