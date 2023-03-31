﻿using System;
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
        private int amount;
        private float duration;
        public UnityEngine.UI.Image icon;
        
        private float currentTime;
        public Action<Character.Character> Apply;
        public Action<Character.Character> Remove;
        public Action<Character.Character> Dots;
        public SPC(float duration, int amount, Action<Character.Character> apply,Action<Character.Character> remove)
        {
            currentTime = this.duration=duration;
            this.amount = amount;
            Apply = apply;
            Remove = remove;
        }
        public SPC(float duration, int amount, Action<Character.Character> dots)
        {
            currentTime = this.duration=duration;
            this.amount = amount;
            Dots = dots;
        }
        
        public void Activation(Character.Character character)
        {
            if(currentTime>0)
            {
                currentTime -= Time.deltaTime;
                //icon.fillAmount = currentTime * (1/duration);
                Dots(character);
            }
            else
            {
                character.RemoveBuff(this);
            }
        }

    }
}