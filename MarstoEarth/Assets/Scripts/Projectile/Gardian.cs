using System.Collections;
using UnityEngine;

namespace Projectile
{
    public class Gardian : Installation
    {
        private AudioSource sound;
        public void Init(int lm, float dg, float rg, float sp)
        {
            base.Init(lm, dg, rg, 0, sp, false);
            sound = Instantiate(SpawnManager.Instance.effectSound, transform);



        }
        private void Awake()
        {
            StartCoroutine(attack(2 / speed));
        }

        private void OnEnable()
        {
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.gravity, sound);
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
                    {
                        target.Hit(transform.position, dmg, 0);
                        AudioManager.Instance.PlayEffect((int)CombatEffectClip.buzz, sound);
                    }
                }
            }
        }
    }
}