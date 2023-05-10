using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : Singleton<MapManager>
{
    public static MapInfo mapInfo;
    public MapGenerator mapGenerator;
    public TMP_InputField inputField;

    public static List<NodeInfo> nodes;
    public static NodeInfo bossNode;
    public static List<PathController> paths;
    public static List<GameObject> walls;
    public Transform nodesTF;

    public bool isMapGenerateFinished = false;

    protected override void Awake()
    {
        nodes = new List<NodeInfo>();
        paths = new List<PathController>();
        walls = new List<GameObject>();
        base.Awake();
        TestInitMapInfo();
    }

    void Start()
    {
        GenerateNewSeed();
        GenerateMapCall();
        InactiveMap();
        isMapGenerateFinished = true;
    }

    public void InactiveMap()
    {
    }

    public void GenerateMapCall()
    {
        mapGenerator.GenerateMap();
        GenerateNavMesh();
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
        mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber = 12;
    }
    public void ChangeRoomNumber(string roomNumber)
    {
        mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber = int.Parse(roomNumber);
    }
    public void ChangeSeedNumber(string seedNumber)
    {
        mapInfo.seed_Number = int.Parse(seedNumber);
    }
    public void ResetNodes()
    {
        NodesDestroy();
        PathsDestroy();
        WallsDestroy();
        nodesTF.transform.rotation = Quaternion.Euler(0, 0, 0);
        NavMesh.RemoveAllNavMeshData();
    }
    public void NodesDestroy()
    {
        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            Destroy(nodes[i].gameObject);
        }
        nodes.Clear();
        mapGenerator.NodeClear();
    }
    public void PathsDestroy()
    {
        for (int i = paths.Count - 1; i >= 0; i--)
        {
            Destroy(paths[i].gameObject);
        }
        paths.Clear();
    }
    public void WallsDestroy()
    {
        for (int i = walls.Count - 1; i >= 0; i--)
        {
            Destroy(walls[i].gameObject);
        }
        walls.Clear();
    }
    public void GenerateNewSeed()
    {
        mapInfo.seed_Number = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        try
        {
            inputField.text = mapInfo.seed_Number.ToString();
        }
        catch (NullReferenceException)
        {
            // edit씬 아닐 경우
        }
    }

    public void GenerateNavMesh()
    {
        /*
        foreach (NodeInfo node in nodes)
        {
            Debug.Log("nodes navmesh");
            GameObject temp = node.gameObject;
            NavMeshSurface surface = temp.GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }
        foreach (GameObject path in paths)
        {
            Debug.Log("paths navmesh");
            NavMeshSurface surface = path.GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }
        */
        nodesTF.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    public void ResetNavMesh()
    {
        NavMesh.RemoveAllNavMeshData();
        nodesTF.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void UpdateGate()
    {
        foreach(PathController path in paths)
        {
            path.UpdateGate();
        }
    }

    public void UpdateGateMesh()
    {
        foreach (PathController path in paths)
        {
            Debug.Log("왜안돼");
            path.CheckNeighborNode();
        }
    }

    public void CloseAllGate()
    {
        foreach (PathController path in paths)
        {
            path.CloseGate();
        }
    }
}
