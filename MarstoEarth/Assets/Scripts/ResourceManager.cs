using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public List<Item.Item> items;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    
}
