using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyPool
{
    Normal,
    Elite,
    Boss,
    Desert
}

public enum EnemyType
{
    cr0,
    cr42,
    cr43,
    cr44,
    kamikaze,
    titan
}

public enum EliteType
{
    cr0,
    kamikaze
}
public enum BossType
{
    titan
}

public enum NormalType
{
    cr42, cr43, cr44
}

public enum desertPool
{
    cr0, cr42, cr43
}

public enum SkillName
{
    Roll,
    Stimpack,
    Grenade,
    Gardian,
    GravityBomb,
    SpiderMine,
    Hyperion,
    Boomerang,
    Distortion,
    AegisBarrier,
    MassShooting,
    Block,
    Charge,
}

public enum OnlyMonsterSkill
{
    Smash,
    Bite,
}

public enum CommonSPC
{
    stun,
    slow,
    bleeding
}



public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public Item.ItemInfo[] itemInfos;
    public MeshFilter[] projectileMesh;
    public SkillInfo[] skillInfos;
    public SkillInfo[] OnlyMonsterSkillInfos;
    public Character.Monster[] enemys;
    public List<Skill.Skill> skills = new();
    public Sprite[] commonSPCIcon;

    protected override void Awake()
    {
        base.Awake();
        //skillInfos = Resources.LoadAll<SkillInfo>("SkillsStat");

        skills.Add(new RollSkill());//플레이어 전용
        skills.Add(new StimPackSkill());
        skills.Add(new GrenadeSkill());
        skills.Add(new GardianSkill());
        skills.Add(new GravityBombSkill());
        skills.Add(new SpiderMineSkill());
        skills.Add(new HyperionSkill());
        skills.Add(new BoomerangSkill());
        skills.Add(new DistortionSkill());
        skills.Add(new AegisBarrierSkill());
        skills.Add(new MassShootingSkill());
        skills.Add(new BlockSkill());
        skills.Add(new ChargeSkill());//플레이어 전용

        DontDestroyOnLoad(gameObject);
    }
    //public static T DeepCopy<T>(T obj)
    //{
    //    using (var stream = new MemoryStream())
    //    {
    //        var formatter = new BinaryFormatter();
    //        formatter.Serialize(stream, obj);
    //        stream.Seek(0, SeekOrigin.Begin);
    //        return (T)formatter.Deserialize(stream);
    //    }
    //}
}