using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Fire : Installation
    {
        private SPC fire;
        private AudioSource sound;
        private void Awake()
        {

            fire = new SPC((ch) => fire.Tick((stack) => ch.Hit(ch.transform.position, dmg * stack, 0)),
                ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.bleeding]);
            sound = Instantiate(SpawnManager.Instance.effectSound, transform);
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.fire, sound);

        }
        
        
        
        void Update()
        {
            BaseUpdate();

            int count = Physics.OverlapSphereNonAlloc(thisTransform.position, range, colliders,
                layerMask);
            for (int i = 0; i < count; i++)
            {
                colliders[i].TryGetComponent(out target);
                if (!target) continue;
                fire.Init(duration * 0.2f);
                target.AddBuff(fire);
            }
        }
    }
}