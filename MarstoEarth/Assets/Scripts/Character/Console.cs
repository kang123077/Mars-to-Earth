using UnityEngine;

namespace Character
{
    public class Console : Character
    {
        [SerializeField] private float optanium;
        [SerializeField] private float experience;

        private float eleapse;
        [SerializeField]private float spawnInterval;

        private void Update()
        {
            eleapse += Time.deltaTime;
            if (eleapse > spawnInterval)
            {
                eleapse -= spawnInterval;
            }
        }

        
    }
}
