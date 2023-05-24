using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class GravityEffect : Installation
    {
        private Skill.SPC stun;
        private AudioSource sound;
        public void Init(float dr, float dg, float rg, int lm, bool enforce)
        {
            base.Init(lm, dr, dg, rg, 0,enforce);
            sound = Instantiate(SpawnManager.Instance.effectSound, transform);
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.gravity, sound);
            sound.loop = true;
            stun = new Skill.SPC((ch) => { ch.stun = true; }, (ch) => { ch.stun = false; },
                ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);
        }

        private void OnEnable()
        {
            sound.Play();
            if (enforce )
            {
                stun.Init(5);
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
                if (target)
                {
                    target.impact += dmg * Time.deltaTime *
                                     (thisTransform.position - colliders[i].transform.position).normalized;
                    
                }
            }
        }
    }
}