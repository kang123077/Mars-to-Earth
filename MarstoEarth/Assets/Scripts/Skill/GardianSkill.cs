using Character;
using System;
using UnityEngine;

namespace Skill
{
    public class GardianSkill : Skill
    {
        readonly static Vector3[] points = new Vector3[3]
        {
            Vector3.forward,
            new Vector3(0.866f,0,-0.5f),
            new Vector3(-0.866f,0,-0.5f),
        };
        public GardianSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Gardian]; 
        }
        protected override bool Activate()
        {
            caster.PlaySkillClip(this);
            
            return true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {

            GameObject gardianSlot = new ();
            gardianSlot.SetActive(false);
            Projectile.Gardians gardians= gardianSlot.AddComponent<Projectile.Gardians>();
            gardians.Init(caster.transform, skillInfo.duration + caster.duration * 0.5f,
                skillInfo.speed + caster.speed * 0.5f);
            for (int i = 0; i < 3; i++)
            {
                GameObject sattlliteSlot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sattlliteSlot.transform.position = gardianSlot.transform.position+points[i]*skillInfo.range;
                sattlliteSlot.transform.LookAt(gardianSlot.transform.position);
                sattlliteSlot.transform.SetParent(gardianSlot.transform);

                Projectile.Gardian satllite = sattlliteSlot.AddComponent<Projectile.Gardian>();
                satllite.Init(caster.layerMask,skillInfo.dmg + caster.dmg * 0.5f,skillInfo.range + caster.range * 0.5f,skillInfo.speed+caster.speed*0.2f)  ;
                
            }
            gardianSlot.SetActive(true);

        }
    }
}