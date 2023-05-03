
using UnityEngine;

namespace Skill
{
    public class ReleaseEffect:MonoBehaviour
    {
        public ParticleSystem refParticle;
        public float duration;
        public float eleapse;       
        
        public void Init(float dr, Vector3 sc)
        {
            transform.localScale= sc;
            duration= dr;
        }

        void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            SpawnManager.Instance.effectPool.Add(this);
        }
        private void Update()
        {
            if (duration <0) return;
            eleapse += Time.deltaTime;
            if (eleapse < duration) return;
            eleapse = 0;
            OnParticleSystemStopped();
        }
    }
}