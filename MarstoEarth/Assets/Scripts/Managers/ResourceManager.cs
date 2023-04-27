using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    Smash,
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
    Bite,
    Block,
    Charge,
    stun,
    slow,
}

public enum CommonSPC
{
    stun,
    slow,
}


public class ResourceManager : Singleton<ResourceManager>
{
    public UnityEngine.UI.Slider hpBar;
    public Item.Item[] items;
    public MeshFilter[] projectileMesh;
    public SkillInfo[] skillInfos;
    public Character.Monster[] enemys;
    public List<Skill.Skill> skills = new();

    public Sprite[] commonSPCIcon;

    protected override void Awake()
    {
        base.Awake();
        //skillInfos = Resources.LoadAll<SkillInfo>("SkillsStat");

        skills.Add(new RollSkill(skillInfos[(int)SkillName.Roll]));//플레이어 전용
        skills.Add(new SmashSkill(skillInfos[(int)SkillName.Smash]));
        skills.Add(new StimPackSkill(skillInfos[(int)SkillName.Stimpack]));
        skills.Add(new GrenadeSkill(skillInfos[(int)SkillName.Grenade]));
        skills.Add(new GardianSkill(skillInfos[(int)SkillName.Gardian]));
        skills.Add(new GravityBombSkill(skillInfos[(int)SkillName.GravityBomb]));
        skills.Add(new SpiderMineSkill(skillInfos[(int)SkillName.SpiderMine]));
        skills.Add(new HyperionSkill(skillInfos[(int)SkillName.Hyperion]));
        skills.Add(new BoomerangSkill(skillInfos[(int)SkillName.Boomerang]));
        skills.Add(new DistortionSkill(skillInfos[(int)SkillName.Distortion]));
        skills.Add(new AegisBarrierSkill(skillInfos[(int)SkillName.AegisBarrier]));
        skills.Add(new MassShootingSkill(skillInfos[(int)SkillName.MassShooting]));
        skills.Add(null);// bite는 몬스터 전용스킬
        skills.Add(new BlockSkill(skillInfos[(int)SkillName.Block]));
        skills.Add(new ChargeSkill(skillInfos[(int)SkillName.Charge]));//플레이어 전용

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