using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public MapInfo mapInfo;
    public MapGenerator mapGenerator;

    protected override void Awake()
    {
        base.Awake();
        TestInitMapInfo();
    }

    void Start()
    {
        mapGenerator.GenerateMap(mapInfo);
    }

    public void TestInitMapInfo()
    {
        mapInfo = gameObject.AddComponent<MapInfo>();
        mapInfo.seed_Number = 0;
        mapInfo.difficulty = 0;
        mapInfo.cur_Dungeon = gameObject.AddComponent<TestDungeonInfo>();
        mapInfo.cur_Dungeon.dungeonName = DungeonName.Mars;
        mapInfo.cur_Dungeon.curStage = 0;
        mapInfo.cur_Dungeon.stageInfo = new StageInfo[2];
        mapInfo.cur_Dungeon.stageInfo[0] = gameObject.AddComponent<StageInfo>();
        mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber = 100;
    }
}
