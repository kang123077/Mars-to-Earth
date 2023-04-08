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
    public List<MeshFilter> projectileMesh;
    public SkillInfo[] skillInfos;
    public List<Skill.Skill> skills= new();

    protected override void Awake()
    {
        base.Awake();
        //skillInfos = Resources.LoadAll<SkillInfo>("SkillsStat");

        int i = 0;
        skills.Add(new RollSkill(skillInfos[i++]));
        skills.Add(new SmashSkill(skillInfos[i++]));
        skills.Add(new StimPackSkill(skillInfos[i++]));
        skills.Add(new GrenadeSkill(skillInfos[i++]));
        skills.Add(new GardianSkill(skillInfos[i++]));
        skills.Add(new GravityBombSkill(skillInfos[i++]));
        skills.Add(new SpiderMineSkill(skillInfos[i++]));
        skills.Add(new ChargeShotSkill(skillInfos[i++]));
        skills.Add(new HyperionSkill(skillInfos[i++]));
        DontDestroyOnLoad(gameObject);
    }

}