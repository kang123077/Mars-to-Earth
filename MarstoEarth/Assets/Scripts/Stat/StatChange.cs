using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public abstract class StatChange
    {
        public StatInfo statInfo;

        public abstract void Effect();
    }
}

