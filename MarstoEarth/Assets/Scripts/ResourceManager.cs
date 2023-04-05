using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public List<Item.Item> items;

    public List<Skill.Skill> skills=new ();

    protected override void Awake()
    {
        base.Awake();
        SkillInfo[] skillInfos = Resources.LoadAll<SkillInfo>("SkillsStat");

        int i = 0;
        skills.Add(new Roll(skillInfos[i++]));
        skills.Add(new Smash(skillInfos[i++]));
        skills.Add(new StimPack(skillInfos[i++]));

        DontDestroyOnLoad(gameObject);
    }


}