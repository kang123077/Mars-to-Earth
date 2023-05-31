using UnityEngine;

namespace Item
{
    public enum ItemType
    {
        Heal,
        Boost,
        PowerUp,
    }
    [CreateAssetMenu(fileName = "New ItemInfo", menuName = "ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        public ItemType type;
        public int itemValue;
        public ParticleSystem thisParticle;
        public ParticleSystem targetParticle;
        public Sprite SPC_Sprite;
    }
}
