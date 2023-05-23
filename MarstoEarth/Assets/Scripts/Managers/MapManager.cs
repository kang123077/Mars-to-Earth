using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : Singleton<MapManager>
{
    public TMP_InputField inputField;
    public MapGenerator mapGenerator;
    public Transform nodesTF;
    public NodeInfo bossNode;
    public List<NodeInfo> nodes;
    public List<PathController> paths;
    public List<GameObject> walls;

    protected override void Awake()
    {
        base.Awake();
        if (nodes == null)
        {
            nodes = new List<NodeInfo>();
        }
        if (paths == null)
        {
            paths = new List<PathController>();
        }
        if (walls == null)
        {
            walls = new List<GameObject>();
        }
    }

    private void Update()
    {
        UpdateDifficulty();
    }

    public void GenerateMapCall()
    {
        // 인게임 매니저에서 실행
        InitMapInfo();
        GenerateNewSeed();
        mapGenerator.GenerateMap();
        GenerateNavMesh();
    }

    public void InitMapInfo()
    {
        // 첫 시작시 False임
        if (MapInfo.cur_Stage > 0)
        {
            // 2스테이지부터 더해지는 값
            // retry아니어야 함
            if (MapInfo.isRetry == false)
            {
                MapInfo.difficulty += 1;
                MapInfo.node_num += 1;
                MapInfo.maxDistance += 0.125f;
                MapInfo.seed_Number = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            }
            // false로 초기화
            MapInfo.isRetry = false;
        }
        else
        {
            //cur_Stage == 0 일때 한정 초기화
            MapInfo.seed_Number = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        // MapManager의 Awake시 항상 Stage++
        MapInfo.cur_Stage++;
    }

    private void UpdateDifficulty()
    {
        float increaseRate = 0.1f; // Difficulty가 증가하는 비율
        MapInfo.difficulty += increaseRate * Time.deltaTime;
        MapInfo.cur_Time += Time.deltaTime;
    }

    public void ChangeSeedNumber(string seedNumber)
    {
        MapInfo.seed_Number = int.Parse(seedNumber);
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
        // Genearate하진않고 그냥 UI에 반영함
        try
        {
            inputField.text = MapInfo.seed_Number.ToString();
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
        foreach (PathController path in paths)
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
