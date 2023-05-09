using System;
using UnityEditor;
using UnityEngine;

namespace Skill
{
 
    [CreateAssetMenu(fileName = "New SkillInfo", menuName = "SkillInfo")]
    public class SkillInfo:ScriptableObject
    {        
        public string description;
        public string clipName;
        public byte clipLayer;
        public byte dmg;
        public float duration;
        public byte range;
        public byte cool;
        public byte speed;
        public Sprite icon;
        public ParticleSystem[] effects;
    }
}