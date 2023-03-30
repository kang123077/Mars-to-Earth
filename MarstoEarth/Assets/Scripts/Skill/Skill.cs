using UnityEngine;

namespace Skill
{
    public enum UsableCharacter
    {
        Rone,
        Miles,
        CR42
    }
    
    public abstract class Skill
    {
        public int skillId;
        public string name;
        public string description;
        public UsableCharacter usableCharacter; 
        protected UnityEngine.UI.Image icon;
        protected float coolDown;
        protected Character.StatInfo statInfo;
        public abstract void Use();
    }
    
}