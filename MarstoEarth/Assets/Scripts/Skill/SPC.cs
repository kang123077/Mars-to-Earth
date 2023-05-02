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
    public class SPC //: ICloneable
    {
        public float duration;
        public Sprite icon;
        public float currentTime;
        public Action<Character.Character> Apply;
        public Action<Character.Character> Remove;
        public Action<Character.Character> Dots;

        public void Init(float duration)
        {
            currentTime = this.duration = duration;
        }
        public SPC(float duration, Action<Character.Character> apply,Action<Character.Character> remove, Sprite icon)
        {
            currentTime = this.duration = duration;
            Apply = apply;
            Remove = remove;
            Dots = null;
            this.icon = icon;
        }
        public SPC(float duration, Action<Character.Character> dots, Sprite icon)
        {
            currentTime = this.duration=duration;
            Dots = dots;
            Apply = null;
            Remove =null;
            this.icon = icon;
        }
        public SPC(float duration,Action<Character.Character> apply, Action<Character.Character> dots,Action<Character.Character> remove, Sprite icon)
        {
            currentTime = this.duration=duration;
            Apply = apply;
            Dots = dots;
            Remove = remove;
            this.icon = icon;
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
        //public object Clone()
        //{
        //    return new SPC(0, Apply, Dots, Remove, icon);
        //}

    }
}