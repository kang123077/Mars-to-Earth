using UnityEngine;

namespace Character
{
    public class Console : Character
    {
        [SerializeField] private float optanium;
        [SerializeField] private float experience;

<<<<<<< HEAD
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

=======
>>>>>>> 03a7e896ba574be499fda3cd9c47bea131f81603
        
    }
}
