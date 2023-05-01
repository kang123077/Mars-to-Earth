
using UnityEngine;
using UnityEngine.AI;

namespace Projectile
{
    public class AegisBarrier : Installation
    {
       
        private static readonly Vector3[] ports = new Vector3[8]
        {
            new (-.875f, .5f, .0f),
            new (-.625f, .5f, .0f),
            new (-.375f, .5f, .0f),
            new (-.125f, .5f, .0f),
            new (.125f, .5f, .0f),
            new (.375f, .5f, .0f),
            new (.625f, .5f, .0f),
            new (.875f, .5f, .0f),
        };

        private Vector3 targetPoint;
        //private ParticleSystem[] effects = new ParticleSystem[8];
        private Projectile pj;
        private float curEleapse;
        private static float pauseEffectEleapse=0.5f;
        private bool pause;
        private NavMeshAgent casterCh;
        private Vector3 startPoint;
        public void Init(int lm, float dg, float rg, float dr, float sp , Transform caster)
        {            
            base.Init(lm, dg, rg, dr, sp);
            var ot = gameObject.AddComponent<NavMeshObstacle>();
            caster.TryGetComponent(out casterCh);
            casterCh.enabled = false;
            Vector3 forward = caster.transform.forward;
            forward.y = 0;
            startPoint =transform.position = caster.transform.position - forward*2;
            for (int i = 0; i < 8; i++)
            {
                GameObject port = Instantiate(ResourceManager.Instance.skillInfos[(int)SkillName.AegisBarrier].effects[^1]).gameObject;
                port.layer = 4;
                port.transform.localScale = range * 0.2f*Vector3.one;
                port.transform.position = transform.position + range * ports[i];
                port.transform.SetParent(transform);
            }
            transform.forward = forward;
            targetPoint = thisTransform.position + thisTransform.forward * range;
            ot.size = new Vector3(range*1.8f, 18, range*0.2f);
        }
       
        private void Update()
        {
            BaseUpdate();
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, targetPoint, speed * Time.deltaTime);
            if (!pause)
            {
                curEleapse += Time.deltaTime;
                if (curEleapse > pauseEffectEleapse)
                    casterCh.enabled = pause = true;
            }
            int size = Physics.OverlapBoxNonAlloc(thisTransform.position, new Vector3(range, 10, range*0.15f), colliders, Quaternion.LookRotation(thisTransform.forward), layerMask|(1<<8));
            for (int i = 0; i < size; i++)
            {
                if (colliders[i].gameObject.layer == 8)
                {                    
                    colliders[i].TryGetComponent(out pj);
                    if (pj.thisInfo[0].lm !=layerMask)
                        Destroy(pj.gameObject);
                }
                else
                {
                    colliders[i].TryGetComponent(out target);
                    target.impact += (target.transform.position - startPoint).normalized*dmg;
                }
                
            }
        }
    }
}