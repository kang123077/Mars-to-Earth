using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "new CharacterStat",menuName = "Character/StatInfo")]
    public class StatInfo : ScriptableObject
    {
        public float dmg;
        public float speed;
        public float def;
        public float duration;
        public float maxHP;
        public float range;
        public float sightLength;
        public float viewAngle;
    }
}