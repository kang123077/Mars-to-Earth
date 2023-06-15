using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum NodePool
{
    All,
    Inside,
    Outside
}

public enum Node
{
    Test_00,
    SFRoom_00,
    YellowRock_00,
    SFWareHouse_00
}

public enum InsideNodePool
{
    SFRoom_00,
    SFWareHouse_00
}

public enum OutsideNodePool
{
    Test_00,
    YellowRock_00
}

public enum PathPool
{
    All,
    Inside,
    Outside
}

public enum Paths
{
    TestGate
}

public enum WallPool
{
    All,
    Inside,
    Outside
}

public enum Wall
{
    TestWall,
    BrokenGate_00,
    YellowRock_00
}

public class NodeGenerator : MonoBehaviour
{
    public GameObject[] nodes;
    public GameObject[] paths;
    public GameObject[] walls;
    public float nodeSpacing;
    public Transform nodeParentTF;
    public Material bossMaterial;

    private void Awake()
    {
        nodeSpacing = 60f;
    }

    /// <summary>
    /// 깊이우선탐색 방식으로 작동하는 노드 생성 함수
    /// </summary>
    /// <param name="x">해당 노드의 x좌표 (음수 가능)</param>
    /// <param name="y">해당 노드의 y좌표 (음수 가능)</param>
    /// <param name="distance">부모 노드로부터의 거리, DFS의 depth</param>
    /// <param name="parentDir">부모의 방향, 체크 시 배제하기 위한 용도 (nullable), East = 0, West = 1, South = 2, North = 3</param>
    /// <returns></returns>
    public NodeInfo GenerateNodes(int x, int y, int distance, int? parentDir, int seed)
    {
        // 최초 진입 시 seed_number로 랜덤 초기화 seed_number가 0일 경우 초기화 X
        // 노드 생성기에서 사용 할 새로운 Random 생성
        if (MapInfo.seed_Number != 0 && seed == 0)
        {
            Random.InitState(MapInfo.seed_Number);
        }
        // 최초 진입 아닐 시 seed로 랜덤 초기화
        if (seed != 0)
        {
            Random.InitState(seed);
        }
        // 현재 노드의 갯수가 정해진 노드의 갯수보다 많거나 같아지면 or 시작 노드로부터 거리가 n 이상이면 return
        if (MapManager.Instance.nodes.Count >= MapInfo.node_num || distance > 10)
        {
            return null;
        }
        // 노드 인스턴스, 노드에서 랜덤으로 결정, 맵인포의 노드풀을 사용
        GameObject nodeObject = Instantiate(
            nodes[NodePoolCheck(MapInfo.cur_NodePool)],
            nodeParentTF);
        nodeObject.transform.position = new Vector3(x * nodeSpacing, 0, y * nodeSpacing);
        nodeObject.name = "nodePrefab " + x.ToString() + ", " + y.ToString();
        NodeInfo nodeInfo = nodeObject.GetComponent<NodeInfo>();
        nodeInfo.x = x; nodeInfo.y = y; nodeInfo.distance = distance;
        // nodeInfo에 node추가 ( nodes.Count 증가 )
        MapManager.Instance.nodes.Add(nodeInfo);
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
            CheckDirection(x, y, distance, nodeInfo, selectedDirection, NextSeed);
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

    public int NodePoolCheck(NodePool nodePool)
    {
        switch (nodePool)
        {
            case NodePool.All:
                Node[] AllNodes = (Node[])Enum.GetValues(typeof(Node));
                Node AllNode = AllNodes[Random.Range(0, AllNodes.Length)];
                return (int)Enum.Parse(typeof(Node), AllNode.ToString());
            case NodePool.Inside:
                InsideNodePool[] insideNodes = (InsideNodePool[])Enum.GetValues(typeof(InsideNodePool));
                InsideNodePool insideNode = insideNodes[Random.Range(0, insideNodes.Length)];
                return (int)Enum.Parse(typeof(Node), insideNode.ToString());
            case NodePool.Outside:
                OutsideNodePool[] outsideNodes = (OutsideNodePool[])Enum.GetValues(typeof(OutsideNodePool));
                OutsideNodePool outsideNode = outsideNodes[Random.Range(0, outsideNodes.Length)];
                return (int)Enum.Parse(typeof(Node), outsideNode.ToString());
            default:
                return 0;
        }
    }

    public int PathPoolCheck(PathPool pathPool)
    {
        switch (pathPool)
        {
            case PathPool.All:
                Paths[] AllPaths = (Paths[])Enum.GetValues(typeof(Paths));
                Paths AllPath = AllPaths[Random.Range(0, AllPaths.Length)];
                return (int)Enum.Parse(typeof(Paths), AllPath.ToString());
            /*
        case PathPool.Inside:
            InsideNodePool[] insideNodes = (InsideNodePool[])Enum.GetValues(typeof(InsideNodePool));
            InsideNodePool insideNode = insideNodes[Random.Range(0, insideNodes.Length)];
            return (int)Enum.Parse(typeof(Node), insideNode.ToString());
        case PathPool.Outside:
            OutsideNodePool[] outsideNodes = (OutsideNodePool[])Enum.GetValues(typeof(OutsideNodePool));
            OutsideNodePool outsideNode = outsideNodes[Random.Range(0, outsideNodes.Length)];
            return (int)Enum.Parse(typeof(Node), outsideNode.ToString());
            */
            default:
                return 0;
        }
    }

    public int WallPoolCheck(WallPool wallPool)
    {
        switch (wallPool)
        {
            case WallPool.All:
                Wall[] AllNodes = (Wall[])Enum.GetValues(typeof(Wall));
                Wall Allnode = AllNodes[Random.Range(0, AllNodes.Length)];
                return (int)Enum.Parse(typeof(Wall), Allnode.ToString());
            /*
        case WallPool.Inside:
            InsideNodePool[] insideNodes = (InsideNodePool[])Enum.GetValues(typeof(InsideNodePool));
            InsideNodePool insideNode = insideNodes[Random.Range(0, insideNodes.Length)];
            return (int)Enum.Parse(typeof(Node), insideNode.ToString());
        case WallPool.Outside:
            OutsideNodePool[] outsideNodes = (OutsideNodePool[])Enum.GetValues(typeof(OutsideNodePool));
            OutsideNodePool outsideNode = outsideNodes[Random.Range(0, outsideNodes.Length)];
            return (int)Enum.Parse(typeof(Node), outsideNode.ToString());
            */
            default:
                return 0;
        }
    }

    public int WallPoolCheck(Wall[] wallPool)
    {
        Wall[] AllWalls = (Wall[])Enum.GetValues(typeof(Wall));
        Wall wall = wallPool[Random.Range(0, wallPool.Length)];
        return (int)Enum.Parse(typeof(Wall), wall.ToString());
    }

    public void CreatePathWall()
    {
        foreach (NodeInfo node in MapManager.Instance.nodes)
        {
            if (node.wallPool != null)
            {
                Wall[] wallPool = node.wallPool;
                if (node.east == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(wallPool)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x + (nodeSpacing / 2f) - 2f, 0, node.transform.position.z);
                    wallObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
                if (node.west == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(wallPool)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x - (nodeSpacing / 2f) + 2f, 0, node.transform.position.z);
                    wallObject.transform.rotation = Quaternion.Euler(0, -90f, 0);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
                if (node.south == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(wallPool)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x, 0, node.transform.position.z - (nodeSpacing / 2f) + 2f);
                    wallObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
                if (node.north == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(wallPool)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x, 0, node.transform.position.z + (nodeSpacing / 2f) - 2f);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
            }
            else
            {
                if (node.east == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(WallPool.All)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x + (nodeSpacing / 2f) - 2f, 0, node.transform.position.z);
                    wallObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
                if (node.west == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(WallPool.All)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x - (nodeSpacing / 2f) + 2f, 0, node.transform.position.z);
                    wallObject.transform.rotation = Quaternion.Euler(0, -90f, 0);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
                if (node.south == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(WallPool.All)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x, 0, node.transform.position.z - (nodeSpacing / 2f) + 2f);
                    wallObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
                if (node.north == null)
                {
                    GameObject wallObject = Instantiate(walls[WallPoolCheck(WallPool.All)], node.transform, true);
                    wallObject.transform.position = new Vector3(node.transform.position.x, 0, node.transform.position.z + (nodeSpacing / 2f) - 2f);
                    ChangeLayerRecursively(wallObject.transform, node.isInside);
                    MapManager.Instance.walls.Add(wallObject);
                }
            }
        }
    }

    void ChangeLayerRecursively(Transform transform, bool isInsideNode)
    {
        if (!isInsideNode)
            return;
        int inObstacleLayer = 14;
        transform.gameObject.layer = inObstacleLayer;

        foreach (Transform child in transform)
        {
            ChangeLayerRecursively(child, isInsideNode);
        }
    }

    /// <summary>
    /// PathNode 생성
    /// </summary>
    /// <param name="nodeInfo">현재 PathNode를 생성중인 Node</param>
    public PathController GeneratePath(NodeInfo nodeInfo, NodeInfo neighbor, Direction direction)
    {
        PathController pathController = Instantiate(paths[PathPoolCheck(PathPool.All)], nodeParentTF).GetComponent<PathController>();
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
        MapManager.Instance.paths.Add(pathController);
        return pathController;
    }

    public void CheckDirection(int x, int y, int distance, NodeInfo nodeInfo, string dir, int seed)
    {
        switch (dir)
        {
            case "East":
                // 위치 확인
                NodeInfo eastNeighbor = MapManager.Instance.nodes.Find(n =>
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
                        MapManager.Instance.nodes.Count < MapInfo.node_num)
                    {
                        NodeInfo newNode = GenerateNodes(x + 1, y, distance + 1, 1, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.East);
                        nodeInfo.east = newNode;
                    }
                }
                break;
            case "West":
                NodeInfo westNeighbor = MapManager.Instance.nodes.Find(n =>
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
                        MapManager.Instance.nodes.Count < MapInfo.node_num)
                    {
                        NodeInfo newNode = GenerateNodes(x - 1, y, distance + 1, 0, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.West);
                        nodeInfo.west = newNode;
                    }
                }
                break;
            case "South":
                NodeInfo southNeighbor = MapManager.Instance.nodes.Find(n =>
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
                        MapManager.Instance.nodes.Count < MapInfo.node_num)
                    {
                        NodeInfo newNode = GenerateNodes(x, y - 1, distance + 1, 3, seed);
                        PathController pathController = GeneratePath(nodeInfo, newNode, Direction.South);
                        nodeInfo.south = newNode;
                    }
                }
                break;
            case "North":
                NodeInfo northNeighbor = MapManager.Instance.nodes.Find(n =>
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
                        MapManager.Instance.nodes.Count < MapInfo.node_num)
                    {
                        NodeInfo newNode = GenerateNodes(x, y + 1, distance + 1, 2, seed);
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
                NodeInfo eastNeighbor = MapManager.Instance.nodes.Find(n => Mathf.Approximately(n.transform.position.x, (x + 1) * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
                // 기억
                if (eastNeighbor != null)
                    nodeInfo.east = eastNeighbor;
                break;
            case "West":
                NodeInfo westNeighbor = MapManager.Instance.nodes.Find(n => Mathf.Approximately(n.transform.position.x, (x - 1) * nodeSpacing)
                                                    && Mathf.Approximately(n.transform.position.z, y * nodeSpacing));
                if (westNeighbor != null)
                    nodeInfo.west = westNeighbor;
                break;
            case "South":
                NodeInfo southNeighbor = MapManager.Instance.nodes.Find(n => Mathf.Approximately(n.transform.position.x, x * nodeSpacing)
                                            && Mathf.Approximately(n.transform.position.z, (y - 1) * nodeSpacing));
                if (southNeighbor != null)
                    nodeInfo.south = southNeighbor;
                break;
            case "North":
                NodeInfo northNeighbor = MapManager.Instance.nodes.Find(n => Mathf.Approximately(n.transform.position.x, x * nodeSpacing)
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
        float probability = 1.0f - (distance / MapInfo.maxDistance);

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
        MapManager.Instance.bossNode = startNode;
        foreach (NodeInfo node in MapManager.Instance.nodes)
        {
            if (node.distance >= MapManager.Instance.bossNode.distance)
            {
                MapManager.Instance.bossNode = node;
            }
        }
        MapManager.Instance.bossNode.isBossNode = true;
        // MapManager.Instance.bossNode.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = bossMaterial;
    }
}
