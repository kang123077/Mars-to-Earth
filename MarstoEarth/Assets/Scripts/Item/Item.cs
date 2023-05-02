using System;
using System.Collections;
using Skill;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        public ItemType type;
        
        private SPC[] spcs;
       

        // public Item()
        // {
        //    
        //     spcs= new SPC[4]
        //     {
        //         new( (ch) => { }, ResourceManager.Instance.itemInfos[1].SPC_Sprite),
        //         new( (ch) => { }, ResourceManager.Instance.itemInfos[1].SPC_Sprite),
        //         new((ch) => { }, ResourceManager.Instance.itemInfos[2].SPC_Sprite),
        //         new( (ch) => { }, ResourceManager.Instance.itemInfos[3].SPC_Sprite),
        //     };
        // }

        
        //        
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