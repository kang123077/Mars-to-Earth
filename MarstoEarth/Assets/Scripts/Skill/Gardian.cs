using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Gardian : Skill
    {
        Vector3[] points = new Vector3[4]
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left
        };
        public Gardian(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            caster.PlaySkillClip(this);
        }

        public override void Effect()
        {

            GameObject gardianSlot = new GameObject();
            gardianSlot.SetActive(false);
            Gardians gardians= gardianSlot.AddComponent<Gardians>();
            gardians.caster = caster.transform;
            gardians.lifeTime = skillInfo.duration;
            gardians.speed = skillInfo.speed;
            
            for (int i = 0; i < 4; i++)
            {
                GameObject sattlliteSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sattlliteSlot.transform.position = gardianSlot.transform.position+points[i];
                sattlliteSlot.transform.SetParent(gardianSlot.transform);
                Satllite satllite = sattlliteSlot.AddComponent<Satllite>();
                satllite.dmg = skillInfo.dmg;
                satllite.layerMask = layerMask;
                satllite.size = skillInfo.size;
            }
            gardianSlot.SetActive(true);
            /*             
            회전하는 오브젝트를 만들고 스피드            
            오브젝트에 가디언즈 오브젝트를 단 오브젝트를 만든다 
            */

        }
    }
}