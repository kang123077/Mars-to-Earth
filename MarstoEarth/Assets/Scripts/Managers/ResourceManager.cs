using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EnemyType
{
    cr42, cr43, cr44,
    scout,
    kamikaze,
    titan,
}

public enum SkillName
{
    Roll,
    Smash,
    Stimpack,
    Grenade,
    Gardian,
    GravityBomb,
    SpiderMine,
    ChargeShot,
    Hyperion,
    Boomerang,
    Distortion,
    AegisBarrier,
    MassShooting,
    Bite
}
public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public Item.Item[] items;
    public MeshFilter[] projectileMesh;
    public SkillInfo[] skillInfos;
    public Character.Character[] enemys;
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
        skills.Add(new BoomerangSkill(skillInfos[i++]));
        skills.Add(new DistortionSkill(skillInfos[i++]));
        skills.Add(new AegisBarrierSkill(skillInfos[i++]));
        skills.Add(new MassShootingSkill(skillInfos[i++]));
        DontDestroyOnLoad(gameObject);
    }

}