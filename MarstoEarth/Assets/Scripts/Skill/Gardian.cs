using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class Gardian : Skill
    {
        readonly static Vector3[] points = new Vector3[3]
        {
            Vector3.forward,
            new Vector3(0.866f,0,-0.5f),
            new Vector3(-0.866f,0,-0.5f),
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

            GameObject gardianSlot = new ();
            gardianSlot.SetActive(false);
            Gardians gardians= gardianSlot.AddComponent<Gardians>();
            gardians.caster = caster.transform;
            gardians.lifeTime = skillInfo.duration+caster.duration*0.5f;
            gardians.speed = skillInfo.speed+caster.speed*0.5f;
            
            for (int i = 0; i < 3; i++)
            {
                GameObject sattlliteSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sattlliteSlot.transform.position = gardianSlot.transform.position+points[i]*skillInfo.range;
                sattlliteSlot.transform.LookAt(gardianSlot.transform.position);
                sattlliteSlot.transform.SetParent(gardianSlot.transform);

                Satllite satllite = sattlliteSlot.AddComponent<Satllite>();
                satllite.layerMask = caster.layerMask;
                satllite.dmg = skillInfo.dmg + caster.dmg * 0.5f;
                satllite.range = skillInfo.range + caster.range * 0.5f;
            }
            gardianSlot.SetActive(true);
            /*             
            회전하는 오브젝트를 만들고 스피드            
            오브젝트에 가디언즈 오브젝트를 단 오브젝트를 만든다 
            */

        }
    }
}