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
            switch (itemInfo.type)
            {
                case ItemType.Heal:
                    player.hp += itemInfo.itemValue;
                    break;
                case ItemType.Shield:
                    player.def += itemInfo.itemValue;
                    break;
                case ItemType.EXP:
                    player.EXP += itemInfo.itemValue;
                    break;
                case ItemType.Optanium:
                    player.optanium += itemInfo.itemValue;
                    break;
            }
            Debug.Log("아이템 사용 이팩트");
            Destroy(gameObject);
        }
        

        
    }
}

