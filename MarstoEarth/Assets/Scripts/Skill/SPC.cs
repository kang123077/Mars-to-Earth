using System;
using UnityEngine;
namespace Skill
{
    public enum conditionType
    {
        atkUp,
        atkDown,
        cunfusion,
        restraint,
        faint
    }
    public class SPC 
    {
        private float duration;
        public UnityEngine.UI.Image icon;
        
        private float currentTime;

        public Action<Character.Character> Apply;
        public Action<Character.Character> Remove;
        public Action<Character.Character> Dots;
        public SPC(float duration, Action<Character.Character> apply,Action<Character.Character> remove)
        {
            currentTime = this.duration=duration;
            Apply = apply;
            Remove = remove;
            Dots = null;
        }
        public SPC(float duration, Action<Character.Character> dots)
        {
            currentTime = this.duration=duration;
            Dots = dots;
            Apply = null;
            Remove =null;
        }
        public SPC(float duration,Action<Character.Character> apply, Action<Character.Character> dots,Action<Character.Character> remove)
        {
            currentTime = this.duration=duration;
            Apply = apply;
            Dots = dots;
            Remove = remove;
        }
        
        public void Activation(Character.Character character)
        {
            if(currentTime>0)
            {
                currentTime -= Time.deltaTime;
                //icon.fillAmount = currentTime * (1/duration);
                Dots?.Invoke(character);
            }
            else
            {
                character.RemoveBuff(this);
            }
        }

    }
}