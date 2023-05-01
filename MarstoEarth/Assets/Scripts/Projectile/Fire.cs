using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Fire : Installation
    {
        private SPC fire;
        private byte timeCount;
        private float curEleapse;
        private static float eleapse=0.2f;
        private void Awake()
        {
            
            fire = new SPC(0, (ch) =>
            {
                curEleapse += Time.deltaTime;
                timeCount++;
                if (curEleapse > eleapse)
                {
                    ch.Hit(ch.transform.position, dmg * Time.deltaTime, 0);
                    curEleapse -= eleapse;
                    timeCount = 0;
                }
                
            }, ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.bleeding]);
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