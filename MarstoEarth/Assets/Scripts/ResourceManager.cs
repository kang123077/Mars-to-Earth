using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public List<Item.Item> items;
    public List<SkillInfo> skillInfos;
    public List<Skill.Skill> skills;
    protected override void Awake()
    {
        base.Awake();
        Resources.LoadAll<SkillInfo>("SkillsStat");
        DontDestroyOnLoad(gameObject);
    }


    
}
