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
    public static List<GameObject> paths;
    public static List<GameObject> walls;
    public Transform nodesTF;

    protected override void Awake()
    {
        nodes = new List<NodeInfo>();
        paths = new List<GameObject>();
        walls = new List<GameObject>();
        base.Awake();
        TestInitMapInfo();
    }

    void Start()
    {
        GenerateNewSeed();
        GenerateMapCall();
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
        mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber = 24;
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
        mapInfo.seed_Number = Random.Range(int.MinValue, int.MaxValue);
        inputField.text = mapInfo.seed_Number.ToString();
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
}
