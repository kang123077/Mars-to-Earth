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
    Bite,
    Block
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

        skills.Add(new RollSkill(skillInfos[(int)SkillName.Roll]));
        skills.Add(new SmashSkill(skillInfos[(int)SkillName.Smash]));
        skills.Add(new StimPackSkill(skillInfos[(int)SkillName.Stimpack]));
        skills.Add(new GrenadeSkill(skillInfos[(int)SkillName.Grenade]));
        skills.Add(new GardianSkill(skillInfos[(int)SkillName.Gardian]));
        skills.Add(new GravityBombSkill(skillInfos[(int)SkillName.GravityBomb]));
        skills.Add(new SpiderMineSkill(skillInfos[(int)SkillName.SpiderMine]));
        skills.Add(new ChargeShotSkill(skillInfos[(int)SkillName.ChargeShot]));
        skills.Add(new HyperionSkill(skillInfos[(int)SkillName.Hyperion]));
        skills.Add(new BoomerangSkill(skillInfos[(int)SkillName.Boomerang]));
        skills.Add(new DistortionSkill(skillInfos[(int)SkillName.Distortion]));
        skills.Add(new AegisBarrierSkill(skillInfos[(int)SkillName.AegisBarrier]));
        skills.Add(new MassShootingSkill(skillInfos[(int)SkillName.MassShooting]));
        skills.Add(null);// bite는 몬스터 전용스킬
        skills.Add(new BlockSkill(skillInfos[(int)SkillName.Block]));
        DontDestroyOnLoad(gameObject);
    }

}