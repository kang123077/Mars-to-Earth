using System;
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
        private int fireEleapse;
        
        private SPC stun;
        private void Awake()
        {
            fire = new SPC((ch) => fire.Tick((stack) => ch.Hit(ch.transform.position, dmg * stack, 0)),
                ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.fire]);
            sound = Instantiate(SpawnManager.Instance.effectSound, transform);
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.fire, sound);
            sound.loop= true;
            stun = new Skill.SPC((ch) => { ch.stun = true; }, (ch) => { ch.stun = false; },
                ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);
        }

        private void OnEnable()
        {
            if (enforce)
            {
                stun.Init(3);
                target.AddBuff(stun);
            }
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
                fireEleapse++;
                if (fireEleapse > 10)
                {
                    fire.Init(duration * 0.2f);
                    target.AddBuff(fire);
                    fireEleapse = 0;
                }
            }
        }
    }
}