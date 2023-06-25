using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public interface IItem
    {
        public abstract void Use(Character.Player player);
    }
}
