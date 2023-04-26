using System;
using UnityEngine;

namespace Skill
{
    //public enum ConditionType
    //{
    //    atkUp,
    //    atkDown,
    //    cunfusion,
    //    restraint,
    //    stun
    //}
    public class SPC 
    {
        private static int IdCount = 0;
        public readonly int id;
        public float duration;
        public int iconNum;
        public float currentTime;
        public Action<Character.Character> Apply;
        public Action<Character.Character> Remove;
        public Action<Character.Character> Dots;
        public bool isStun;

        public void Init(float duration)
        {
            currentTime = this.duration = duration;
        }
        public SPC(float duration, Action<Character.Character> apply,Action<Character.Character> remove, int iconNum)
        {
            currentTime = this.duration = duration;
            Apply = apply;
            Remove = remove;
            Dots = null;
            this.iconNum = iconNum;
            
            id = IdCount++;
        }
        public SPC(float duration, Action<Character.Character> dots,int iconNum)
        {
            currentTime = this.duration=duration;
            Dots = dots;
            Apply = null;
            Remove =null;
            this.iconNum = iconNum;
            
            id = IdCount++;
        }
        public SPC(float duration,Action<Character.Character> apply, Action<Character.Character> dots,Action<Character.Character> remove,int iconNum)
        {
            currentTime = this.duration=duration;
            Apply = apply;
            Dots = dots;
            Remove = remove;
            this.iconNum = iconNum;
            
            id = IdCount++;
        }
        
        public void Activation(Character.Character character)
        {            
            if(currentTime>0)
            {
                currentTime -= Time.deltaTime;
                
                Dots?.Invoke(character);
            }
            else
            {
                character.RemoveBuff(this);
            }
        }

    }
}