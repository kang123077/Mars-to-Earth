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
        
        public string description;
        //public SkillType skillType;
        public string clipName;
        public byte clipLayer;
        public Sprite icon;
        public byte dmg;
        public float duration;
        public byte range;
        public byte cool;
        public byte speed;
        public ParticleSystem[] effects;
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