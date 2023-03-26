using System;
using UnityEditor;
using UnityEngine;

namespace Item
{
    public enum ItemType
    {
        Optanium,
        EXP,
        Heal,
        Shield,
    }
    [CreateAssetMenu(fileName = "New ItemInfo", menuName = "ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        public ItemType type;
        public int itemValue;
        
        
    }
}
