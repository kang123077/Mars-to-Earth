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
    public enum UsableCharacter
    {
        Common,
        Rone,
        Miles,
        CR42
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
        public string skillName;
        public string description;
        public SkillType skillType;
        public UsableCharacter usableCharacter;
        public AnimationClip animationClip;
        public TargetType targetType;
        public UnityEngine.UI.Image icon;
        public float dmg;
        public float range;
        public float effectiveRange;
        public float coolDown;
        public float skillSpeed;
    }
}
