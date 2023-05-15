using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : Singleton<MapManager>
{
    public MapInfo mapInfo;
    public TMP_InputField inputField;

    public List<NodeInfo> nodes;
    public NodeInfo bossNode;
    public List<PathController> paths;
    public List<GameObject> walls;
    public Transform nodesTF;
    public MapGenerator mapGenerator;

    protected override void Awake()
    {
        base.Awake();
        nodes = new List<NodeInfo>();
        paths = new List<PathController>();
        walls = new List<GameObject>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        nodesTF = GameObject.Find("NodeTF").transform;
        TestInitMapInfo();
        DontDestroyOnLoad(this);
        Debug.Log("ManManagerAwake");
    }

    public void GenerateMapCall()
    {
        // 인게임 매니저에서 실행
        InitInfos();
        GenerateNewSeed();
        mapGenerator.GenerateMap();
        GenerateNavMesh();
    }

    public void TestInitMapInfo()
    {
        mapInfo = gameObject.AddComponent<MapInfo>();
        mapInfo.seed_Number = 0;
        mapInfo.difficulty = 0;
        mapInfo.node_num = 4;
    }

    public void ChangeSeedNumber(string seedNumber)
    {
        mapInfo.seed_Number = int.Parse(seedNumber);
    }

    public void InitInfos()
    {
        nodes.Clear();
        paths.Clear();
        walls.Clear();
        bossNode = null;
        mapGenerator = FindObjectOfType<MapGenerator>();
        nodesTF = GameObject.Find("NodeTF").transform;
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
            try
            {
                Destroy(nodes[i].gameObject);
            }
            catch (MissingReferenceException)
            {
                // 다른 씬으로 넘어온 경우
            }
        }
        nodes.Clear();
        mapGenerator.NodeClear();
    }

    public void PathsDestroy()
    {
        for (int i = paths.Count - 1; i >= 0; i--)
        {
            try
            {
                Destroy(paths[i].gameObject);
            }
            catch (MissingReferenceException)
            {
                // 다른 씬으로 넘어온 경우
            }
        }
        paths.Clear();
    }

    public void WallsDestroy()
    {
        for (int i = walls.Count - 1; i >= 0; i--)
        {
            try
            {
                Destroy(walls[i].gameObject);
            }
            catch (MissingReferenceException)
            {
                // 다른 씬으로 넘어온 경우
            }
        }
        walls.Clear();
    }

    public void GenerateNewSeed()
    {
        Debug.Log("GenerateNewSeed");
        Debug.Log(mapInfo.seed_Number);
        mapInfo.seed_Number = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        Debug.Log(mapInfo.seed_Number);
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
