
using System;
using UnityEngine;

namespace Skill
{
    public class ReleaseEffect:MonoBehaviour
    {
        public ParticleSystem refParticle;
        public float duration;
        public float eleapse;
        public AudioSource sound;
        public void Init(float dr, float sc, AudioClip clip)
        {
            transform.localScale= sc* Vector3.one;
            duration= dr;
            sound.clip = clip;
        }

        void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            SpawnManager.Instance.effectPool.Add(this);
        }

        private void OnEnable()
        {
            sound.Play();
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