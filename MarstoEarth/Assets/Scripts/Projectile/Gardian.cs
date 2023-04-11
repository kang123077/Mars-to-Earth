using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Gardian : MonoBehaviour
    {
        private readonly Collider[] colliders = new Collider[5];
        private float dmg;
        private int layerMask;
        private float range;
        private float speed;

        public void Init(int lm, float dg, float rg, float sp)
        {
            layerMask = lm;
            dmg = dg;
            range = rg;
            speed = sp;
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
                if (Physics.OverlapSphereNonAlloc(transform.position, range * 0.1f, colliders,
                        layerMask) > 0)
                {
                    colliders[0].TryGetComponent(out Character.Character target);
                    if (target)
                        target.Hit(transform.position, dmg);
                }
            }
        }
    }
}