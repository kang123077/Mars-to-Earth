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
        public int skillId { get; }
        public string name { get; }
        public string description { get; }
        public UsableCharacter usableCharacter { get; } 
        protected UnityEngine.UI.Image icon { get; }
        protected float coolDown { get; set; }
        [SerializeField]protected Character.StatInfo statInfo;
        public abstract void Use();
    }
    
}