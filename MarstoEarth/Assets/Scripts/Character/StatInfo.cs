
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
    public class staticStat
    {
        public static float dmg;
        public static float speed ;
        public static float def;
        public static float duration;
        public static float maxHP;
        public static float range;
        public static byte count;

        public static void LoadStat(Player player)
        {
            if (count < 1)
            {
                ResetValues(player);
            }

            player.dmg = dmg;
            player.speed = speed;
            player.def = def;
            player.duration = duration;
            player._hp = player.MaxHp = maxHP;
            player.range = range;

        }

        public static void saveStat(Player player)
        {
            count++;
            player.ClearBuff();
            dmg = player.dmg;
            speed = player.speed;
            def = player.def;
            duration = player.duration;
            maxHP = player.MaxHp;
            range = player.range;
        }
        
        public static void ResetValues(Player player)
        {
            count = 0;
            dmg = player.characterStat.dmg;
            speed = player.characterStat.speed;
            def = player.characterStat.def;
            duration = player.characterStat.duration;
            maxHP = player.characterStat.maxHP;
            range = player.characterStat.range;
        }
    }
}