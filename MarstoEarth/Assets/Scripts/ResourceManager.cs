using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public List<Item.Item> items;

    public List<Skill.Skill> skills;
    protected override void Awake()
    {
        base.Awake();
        skills = new List<Skill.Skill>();
        SkillInfo[] skillInfos = Resources.LoadAll<SkillInfo>("SkillsStat");
        foreach(SkillInfo skillInfo in skillInfos)
        {
            skills.Add(new Smash(skillInfo));
        }

        DontDestroyOnLoad(gameObject);

    }


    
}
