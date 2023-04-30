
using UnityEngine;

namespace Skill
{
    public class ReleaseEffect:MonoBehaviour
    {
        
        private ParticleSystem thisParticle;
        
        void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            TryGetComponent(out thisParticle);
            SpawnManager.Instance.effectPool.Add(thisParticle);
        }
    }
}