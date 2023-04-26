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
        //public UnityEngine.UI.Image icon;
        public float currentTime;
        public Action<Character.Character> Apply;
        public Action<Character.Character> Remove;
        public Action<Character.Character> Dots;
        public bool isStun;

        public void Init(float duration)
        {
            currentTime = this.duration = duration;
        }
        public SPC(float duration, Action<Character.Character> apply,Action<Character.Character> remove)
        {
            currentTime = this.duration = duration;
            Apply = apply;
            Remove = remove;
            Dots = null;
            id = IdCount++;
        }
        public SPC(float duration, Action<Character.Character> dots)
        {
            currentTime = this.duration=duration;
            Dots = dots;
            Apply = null;
            Remove =null;
            id = IdCount++;
        }
        public SPC(float duration,Action<Character.Character> apply, Action<Character.Character> dots,Action<Character.Character> remove)
        {
            currentTime = this.duration=duration;
            Apply = apply;
            Dots = dots;
            Remove = remove;
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