using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        [SerializeField]public ItemInfo itemInfo;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Player player)
        {
            player.ApplyItemEffects(this);
            Destroy(gameObject);
        }
        

        
    }
}

