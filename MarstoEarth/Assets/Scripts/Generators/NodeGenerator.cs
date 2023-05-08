using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
// using static UnityEditorInternal.VersionControl.ListControl;

public class NodeGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject nodePrefab2;
    public GameObject pathPrefab;
    public GameObject wallPrefab;
    public float nodeSpacing;
    public Transform nodeParentTF;
    public Material bossMaterial;
    private void Awake()
    {
    }
    void Start()
    {
        nodeSpacing = 60f;
    }
    void Update()
    {
    }
    /// <summary>
    /// 깊이우선탐색 방식으로 작동하는 노드 생성 함수
    /// </summary>
    /// <param name="mapInfo">함수 내에서 참조하기 위한 현재 생성중인 맵 정보</param>
    /// <param name="x">해당 노드의 x좌표 (음수 가능)</param>
    /// <param name="y">해당 노드의 y좌표 (음수 가능)</param>
    /// <param name="distance">부모 노드로부터의 거리, DFS의 depth</param>
    /// <param name="parentDir">부모의 방향, 체크 시 배제하기 위한 용도 (nullable), East = 0, West = 1, South = 2, North = 3</param>
    /// <returns></returns>
    public NodeInfo GenerateNodes(MapInfo mapInfo, int x, int y, int distance, int? parentDir, int seed)
    {
        // 최초 진입 시 seed_number로 랜덤 초기화 seed_number가 0일 경우 초기화 X
        // 노드 생성기에서 사용 할 새로운 Random 생성
        if (mapInfo.seed_Number != 0 && seed == 0)
        {
            Random.InitState(mapInfo.seed_Number);
        }
        // 최초 진입 아닐 시 seed로 랜덤 초기화
        if (seed != 0)
        {
            Random.InitState(seed);
        }
        // 현재 노드의 갯수가 정해진 노드의 갯수보다 많거나 같아지면 or 시작 노드로부터 거리가 n 이상이면 return
        if (MapManager.nodes.Count >= mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber || distance > 10)
        {
            return null;
        }
        // 노드 인스턴스
        GameObject nodeObject = Instantiate(nodePrefab, nodeParentTF);
        nodeObject.transform.position = new Vector3(x * nodeSpacing, 0, y * nodeSpacing);
        nodeObject.name = "nodePrefab " + x.ToString() + ", " + y.ToString();
        NodeInfo nodeInfo = nodeObject.GetComponent<NodeInfo>();
        nodeInfo.x = x; nodeInfo.y = y; nodeInfo.distance = distance;
        // nodeInfo에 node추가 ( nodes.Count 증가 )
        MapManager.nodes.Add(nodeInfo);
        // 방이 각 방향을 체크했는지 나타내는 로컬 리스트
        List<string> directions = new List<string> { "East", "West", "South", "North" };
        // 부모노드가 있을 경우 해당 방향 기억 & 배제
        if (parentDir != null)
        {
            CheckParent(x, y, nodeInfo, directions[(int)parentDir]);
            directions.RemoveAt((int)parentDir);
        }
        // 모든 방향을 체크 할 때 까지
        while (directions.Count != 0)
        {
            // 방향마다 다른 Random으로 초기화
            int NextSeed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(NextSeed);
            // 랜덤으로 체크할 방향 결정
            int randomIndex = Random.Range(0, directions.Count);
            string selectedDirection = directions[randomIndex];
            // 해당 방향 체크 (함수 설명 참조)
            CheckDirection(mapInfo, x, y, distance, nodeInfo, selectedDirection, NextSeed);
            // 해당 방향 삭제
            directions.RemoveAt(randomIndex);
        }
        // 모든 생성이 종료되면 도달 (시작 노드)
        if (distance == 0)
        {
            // 생성 마치고 링크가 없는 노드 방향에 벽 생성
            CreatePathWall();
            // 생성한 모든 노드, 패스, 벽 회전
            // RotateAllNodes();
            CheckBossNode(nodeInfo);
        }
        return nodeInfo;
    }

    public void CreatePathWall()
    {
        foreach (NodeInfo node in MapManager.nodes)
        {
            if (node.east == null)
            {
                GameObject wallObject = Instantiate(wallPrefab, node.transform);
                wallObject.transform.position = new Vector3(node.transform.position.x + (nodeSpacing / 2f) - 2f, 0, node.transform.position.z);
                wallObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
                MapManager.walls.Add(wallObject);
            }
            if (node.west == null)
            {
                GameObject wallObject = Instantiate(wallPrefab, node.transform);
                wallObject.transform.position = new Vector3(node.transform.position.x - (nodeSpacing / 2f) + 2f, 0, node.transform.position.z);
                wallObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
                MapManager.walls.Add(wallObject);
            }
            if (node.south == null)
            {
                GameObject wallObject = Instantiate(wallPrefab, node.transform);
                wallObject.transform.position = new Vector3(node.transform.position.x, 0, node.transform.position.z - (nodeSpacing / 2f) + 2f);
                MapManager.walls.Add(wallObject);
            }
            if (node.north == null)
            {
                GameObject wallObject = Instantiate(wallPrefab, node.transform);
                wallObject.transform.position = new Vector3(node.transform.position.x, 0, node.transform.position.z + (nodeSpacing / 2f) - 2f);
                MapManager.walls.Add(wallObject);
            }
        }
    }

    /// <summary>
    /// PathNode 생성
    /// </summary>
    /// <param name="nodeInfo">현재 PathNode를 생성중인 Node</param>
    public PathController GeneratePath(NodeInfo nodeInfo, NodeInfo neighbor, Direction direction)
    {
        PathController pathController = Instantiate(pathPrefab, nodeParentTF).GetComponent<PathController>();
        switch (direction)
        {
            case Direction.East:
                pathController.transform.position = new Vector3(nodeInfo.transform.position.x + (nodeSpacing / 2f), 0, nodeInfo.transform.position.z);
                pathController.transform.rotation = Quaternion.Euler(0, 90f, 0);
                break;
            case Direction.West:
                pathController.transform.position = new Vector3(nodeInfo.transform.position.x - (nodeSpacing / 2f), 0, nodeInfo.transform.position.z);
                pathController.transform.rotation = Quaternion.Euler(0, -90f, 0);
                break;
            case Direction.South:
                pathController.transform.position = new Vector3(nodeInfo.transform.position.x, 0, nodeInfo.transform.position.z - (nodeSpacing / 2f));
                pathController.transform.rotation = Quaternion.Euler(0, 180f, 0);
                break;
            case Direction.North:
                pathController.transform.position = new Vector3(nodeInfo.transform.position.x, 0, nodeInfo.transform.position.z + (nodeSpacing / 2f));
                break;
            default:
                break;
        }
        pathController.AddNeighbor(nodeInfo, Relation.Parent);
        pathController.AddNeighbor(neighbor, Relation.Children);
        MapManager.paths.Add(pathController);
        return pathController;
    }

    public void CheckDirection(MapInfo mapInfo, int x, int y, int distance, NodeInfo nodeInfo, string dir, int seed)
    {
        switch (dir)
        {
            case "East":
                // 위치 확인
                NodeInfo eastNeighbor = MapManager.nodes.Find(n => 
                Mathf.Approximately(n.transform.position.x, (x + 1) * nodeSpacing) &&
                Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
                // 있으면 확률판정 후에 기억, 패스노드 생성
                if (eastNeighbor != null)
                {
                    if (Random.value > 0.5)
                    {
                        if (eastNeighbor.west != nodeInfo && nodeInfo.east != eastNeighbor)
                        {
                            GeneratePath(nodeInfo, eastNeighbor, Direction.East);
                        }
                        nodeInfo.east = eastNeighbor;
                        eastNeighbor.west = nodeInfo;
                    }
                }
                // 없으면 확률판정 후에 노드, 패스노드 생성, 서로 연결
                else
                {
                    if (ProbabilityBasedOnDistance(distance) &&
                        MapManager.nodes.Count < mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
                    {
                        NodeInfo newNode = GenerateNodes(mapInfo, x + 1, y, distance + 1, 1, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.East);
                        nodeInfo.east = newNode;
                    }
                }
                break;
            case "West":
                NodeInfo westNeighbor = MapManager.nodes.Find(n =>
                Mathf.Approximately(n.transform.position.x, (x - 1) * nodeSpacing) &&
                Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
                if (westNeighbor != null)
                {
                    if (Random.value > 0.5)
                    {
                        if (westNeighbor.east != nodeInfo && nodeInfo.west != westNeighbor)
                        {
                            GeneratePath(nodeInfo, westNeighbor, Direction.West);
                        }
                        nodeInfo.west = westNeighbor;
                        westNeighbor.east = nodeInfo;
                    }
                }
                else
                {
                    if (ProbabilityBasedOnDistance(distance) &&
                        MapManager.nodes.Count < mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
                    {
                        NodeInfo newNode = GenerateNodes(mapInfo, x - 1, y, distance + 1, 0, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.West);
                        nodeInfo.west = newNode;
                    }
                }
                break;
            case "South":
                NodeInfo southNeighbor = MapManager.nodes.Find(n => 
                Mathf.Approximately(n.transform.position.x, x * nodeSpacing) &&
                Mathf.Approximately(n.transform.position.z, (y - 1) * nodeSpacing));
                if (southNeighbor != null)
                {
                    if (Random.value > 0.5)
                    {
                        if (southNeighbor.north != nodeInfo && nodeInfo.south != southNeighbor)
                        {
                            GeneratePath(nodeInfo, southNeighbor, Direction.South);
                        }
                        nodeInfo.south = southNeighbor;
                        southNeighbor.north = nodeInfo;
                    }
                }
                else
                {
                    if (ProbabilityBasedOnDistance(distance) &&
                        MapManager.nodes.Count < mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
                    {
                        NodeInfo newNode = GenerateNodes(mapInfo, x, y - 1, distance + 1, 3, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.South);
                        nodeInfo.south = newNode;
                    }
                }
                break;
            case "North":
                NodeInfo northNeighbor = MapManager.nodes.Find(n =>
                Mathf.Approximately(n.transform.position.x, x * nodeSpacing) &&
                Mathf.Approximately(n.transform.position.z, (y + 1) * nodeSpacing));
                if (northNeighbor != null)
                {
                    if (Random.value > 0.5)
                    {
                        if (northNeighbor.south != nodeInfo && nodeInfo.north != northNeighbor)
                        {
                            GeneratePath(nodeInfo, northNeighbor, Direction.North);
                        }
                        nodeInfo.north = northNeighbor;
                        northNeighbor.south = nodeInfo;
                    }
                }
                else
                {
                    if (ProbabilityBasedOnDistance(distance) &&
                        MapManager.nodes.Count < mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
                    {
                        NodeInfo newNode = GenerateNodes(mapInfo, x, y + 1, distance + 1, 2, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.North);
                        nodeInfo.north = newNode;
                    }
                }
                break;
        }
    }

    public void CheckParent(int x, int y, NodeInfo nodeInfo, string dir)
    {
        switch (dir)
        {
            case "East":
                // 위치 확인
                NodeInfo eastNeighbor = MapManager.nodes.Find(n => Mathf.Approximately(n.transform.position.x, (x + 1) * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
                // 기억
                if (eastNeighbor != null)
                    nodeInfo.east = eastNeighbor;
                break;
            case "West":
                NodeInfo westNeighbor = MapManager.nodes.Find(n => Mathf.Approximately(n.transform.position.x, (x - 1) * nodeSpacing)
                                                    && Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
                if (westNeighbor != null)
                    nodeInfo.west = westNeighbor;
                break;
            case "South":
                NodeInfo southNeighbor = MapManager.nodes.Find(n => Mathf.Approximately(n.transform.position.x, x * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, (y - 1) * nodeSpacing));
                if (southNeighbor != null)
                    nodeInfo.south = southNeighbor;
                break;
            case "North":
                NodeInfo northNeighbor = MapManager.nodes.Find(n => Mathf.Approximately(n.transform.position.x, x * nodeSpacing)
                                                    && Mathf.Approximately(n.transform.position.z, (y + 1) * nodeSpacing));
                if (northNeighbor != null)
                    nodeInfo.north = northNeighbor;
                break;
        }
    }

    // Function that returns a bool value based on the distance
    bool ProbabilityBasedOnDistance(int distance)
    {
        // Calculate the probability using an inverse linear function
        float maxDistance = 4f;
        float probability = 1.0f - (distance / maxDistance);

        // Generate a random value between 0 and 1
        float randomValue = Random.Range(0.0f, 1.0f);

        // Check if the random value is less than or equal to the probability
        if (randomValue <= probability)
        {
            // Return true if the random value is less than or equal to the probability
            return true;
        }
        else
        {
            // Return false if the random value is greater than the probability
            return false;
        }
    }

    public void RotateAllNodes()
    {
        /*
        foreach(NodeInfo node in MapManager.nodes)
        {
            GameObject temp = node.gameObject;
            temp.transform.RotateAround(Vector3.zero, Vector3.up, 45);
        }
        foreach (GameObject path in MapManager.paths)
        {
            path.transform.RotateAround(Vector3.zero, Vector3.up, 45);
        }
        foreach (GameObject wall in MapManager.walls)
        {
            wall.transform.RotateAround(Vector3.zero, Vector3.up, 45);
        }
        */
        // nodeParentTF.RotateAround(Vector3.zero, Vector3.up, 45);
    }

    public int GetRoomNumber(int distance)
    {
        // Define the probability distribution for roomNumber values
        float[] probabilities = { 500f, 400f, 300f, 200f, 100f };

        // Adjust the probabilities based on the distance
        for (int i = 0; i < probabilities.Length; i++)
        {
            probabilities[i] *= Mathf.Pow(0.8f, distance);
        }

        // Normalize the probabilities so that they add up to 1
        float sum = probabilities.Sum();
        for (int i = 0; i < probabilities.Length; i++)
        {
            probabilities[i] /= sum * 100f;
        }

        // Sample from the probability distribution
        float rand = Random.value;
        float accum = 0.0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            accum += probabilities[i];
            if (rand < accum)
            {
                return i;
            }
        }
        // This code should never be reached, but we need to return something
        return 0;
    }

    public void CheckBossNode(NodeInfo startNode)
    {
        MapManager.bossNode = startNode;
        foreach(NodeInfo node in MapManager.nodes)
        {
            if (node.distance >= MapManager.bossNode.distance)
            {
                MapManager.bossNode = node;
            }
        }
        MapManager.bossNode.isBossNode = true;
        MapManager.bossNode.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = bossMaterial;
    }

    /*
    // 배정 된 방향 && 노드 갯수 확인
    if (east == 1 && nodes.Count <= mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
    {
        // 예상 위치
        NodeInfo eastNeighbor = nodes.Find(n => Mathf.Approximately(n.transform.position.x, (x + 1) * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
        if (eastNeighbor != null)
            nodeInfo.east = eastNeighbor;
        else
            nodeInfo.east = GenerateNodes(mapInfo, x + 1, y, distance + 1);
    }
    if (west == 1 && nodes.Count <= mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
    {
        // 예상 위치
        NodeInfo westNeighbor = nodes.Find(n => Mathf.Approximately(n.transform.position.x, (x - 1) * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
        if (westNeighbor != null)
            nodeInfo.west = westNeighbor;
        else
            nodeInfo.west = GenerateNodes(mapInfo, x - 1, y, distance + 1);
    }
    if (south == 1 && nodes.Count <= mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
    {
        // 예상 위치
        NodeInfo southNeighbor = nodes.Find(n => Mathf.Approximately(n.transform.position.x, x * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, (y - 1) * nodeSpacing));
        if (southNeighbor != null)
            nodeInfo.south = southNeighbor;
        else
            nodeInfo.south = GenerateNodes(mapInfo, x, y - 1, distance + 1);
    }
    if (north == 1 && nodes.Count <= mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
    {
        // 예상 위치
        NodeInfo northNeighbor = nodes.Find(n => Mathf.Approximately(n.transform.position.x, x * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, (y + 1) * nodeSpacing));
        if (northNeighbor != null)
            nodeInfo.north = northNeighbor;
        else
            nodeInfo.north = GenerateNodes(mapInfo, x, y + 1, distance + 1);
    }
    */
}
