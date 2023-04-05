using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public List<Item.Item> items;
    public List<GameObject> projectilePrefabs;
    public SkillInfo[] skillInfos;
    public List<Skill.Skill> skills= new();

    protected override void Awake()
    {
        base.Awake();
        //skillInfos = Resources.LoadAll<SkillInfo>("SkillsStat");

        Debug.Log(skillInfos);
        
        Debug.Log(skillInfos[0]);
        int i = 0;
        skills.Add(new Roll(skillInfos[i++]));
        skills.Add(new Smash(skillInfos[i++]));
        skills.Add(new StimPack(skillInfos[i++]));
        skills.Add(new Grenade(skillInfos[i++]));
        
        DontDestroyOnLoad(gameObject);
    }

}