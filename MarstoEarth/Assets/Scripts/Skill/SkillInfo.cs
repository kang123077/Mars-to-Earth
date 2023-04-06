using System;
using UnityEditor;
using UnityEngine;

namespace Skill
{
 
    public enum SkillType
    {
        ActiveSkill,
        PassiveSkill,
    }
    [CreateAssetMenu(fileName = "New SkillInfo", menuName = "SkillInfo")]
    public class SkillInfo:ScriptableObject
    {
        public int skillId;
        public string description;
        public SkillType skillType;
        public string clipName;
        public byte clipLayer;
        public Sprite icon;
        public byte dmg;
        public float duration;
        public byte size;
        public byte coolDown;
        public byte speed;
    }
}

/*  public enum UsableCharacter
    {
        Common,
        Rone,
        Miles,
        CR42
    }
*/