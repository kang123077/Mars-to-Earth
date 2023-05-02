
using UnityEngine;

namespace Skill
{
    public class ReleaseEffect:MonoBehaviour
    {
        public ParticleSystem refParticle;
        
        void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            SpawnManager.Instance.effectPool.Add(this);
        }
    }
}