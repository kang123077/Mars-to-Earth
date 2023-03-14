using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager :Singleton<ResourceManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    
}
