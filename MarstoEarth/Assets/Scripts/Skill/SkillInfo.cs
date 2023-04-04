using System;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    public enum TargetType
    {
        Target,
        NonTarget,
    }
  
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
        public TargetType targetType;
        public string clipName;
        public int clipLayer;
        public UnityEngine.UI.Image icon;
        public float dmg;
        public float size;
        public float coolDown;
        public float speed;
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