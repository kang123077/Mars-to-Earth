using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    
}
