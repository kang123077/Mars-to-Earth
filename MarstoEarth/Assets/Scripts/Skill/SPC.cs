using System;
using UnityEngine;

namespace Skill
{
    public class SPC //: ICloneable
    {
        public float duration;
        public Sprite icon;
        public float currentTime;
        public Action<Character.Character> Apply;
        public Action<Character.Character> Remove;
        public Action<Character.Character> Dots;
        
        private static float eleapse = 0.2f;
        private float curEleapse;
        private int curCount;

        public void Init(float duration)
        {
            currentTime = this.duration = duration;
        }
        public void Init(Sprite icon)
        {
            this.icon = icon;
        }
        public SPC(Action<Character.Character> apply,Action<Character.Character> remove, Sprite icon)
        {
            Apply = apply;
            Remove = remove;
            Dots = null;
            this.icon = icon;
        }
        public SPC( Action<Character.Character> dots, Sprite icon)
        {
            Dots = dots;
            Apply = null;
            Remove =null;
            this.icon = icon;
        }
        public SPC(Action<Character.Character> apply, Action<Character.Character> dots,Action<Character.Character> remove, Sprite icon)
        {
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
        public void Tick(Action<int> action)
        {
            curEleapse += Time.deltaTime;
            curCount++;
            if (curEleapse>eleapse)
            {
                action(curCount);
                curCount = 0;
            }
        }
        //public object Clone()
        //{
        //    return new SPC(0, Apply, Dots, Remove, icon);
        //}

    }
}