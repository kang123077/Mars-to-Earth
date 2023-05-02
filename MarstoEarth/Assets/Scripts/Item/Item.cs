using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        public ItemType type;

        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Player player)
        {
            switch (type)
            {
                case ItemType.Heal:
                    Debug.Log("힐!!@!@!");
                    break;
                case ItemType.Boost:
                    Debug.Log("스피드!!@!@!");
                    break;
                case ItemType.PowerUp:
                    Debug.Log("공격력!!@!@!");
                    break;
                case ItemType.Shield:
                    Debug.Log("방어!!@!@!");
                    break;

               
            }
            Debug.Log("아이템 사용 이팩트");
            SpawnManager.Instance.itemPool.Add(this);
            gameObject.SetActive(false);
        }        
    }
}