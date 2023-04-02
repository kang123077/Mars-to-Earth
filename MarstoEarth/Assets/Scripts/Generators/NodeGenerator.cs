using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public float nodeSpacing;
    public Transform nodeParentTF;

    private List<NodeInfo> nodes;
    private void Awake()
    {
        nodes = new List<NodeInfo>();
    }
    void Start()
    {
        nodeSpacing = 10f;
    }
    void Update()
    {
    }
    public NodeInfo GenerateNodes(MapInfo mapInfo, int x, int y, int distance)
    {
        // 현재 노드의 갯수가 정해진 노드의 갯수보다 많거나 같아지면 return
        if (nodes.Count >= mapInfo.cur_Dungeon.stageInfo[mapInfo.cur_Dungeon.curStage].roomNumber)
        {
            return null;
        }
        // 노드 인스턴스
        GameObject nodeObject = Instantiate(nodePrefab, nodeParentTF);
        nodeObject.transform.position = new Vector3(x * nodeSpacing, 0, y * nodeSpacing);
        nodeObject.name = "nodePrefab " + x.ToString() + ", " + y.ToString();
        NodeInfo nodeInfo = nodeObject.GetComponent<NodeInfo>();
        nodeInfo.x = x; nodeInfo.y = y;
        nodes.Add(nodeInfo);
        // 시작 노드로부터 거리가 5 이하이면
        if (distance <= 4)
        {
            Debug.Log(distance);
            // 이웃 수 정하고 랜덤으로 방향 배정 (가중치 현재는 X)
            int neighbor = Random.Range(1, 5);
            Debug.Log(neighbor);
            int east = 0, west = 0, south = 0, north = 0;
            while (east + west + south + north != neighbor)
            {
                east = Random.Range(0, 2);
                west = Random.Range(0, 2);
                south = Random.Range(0, 2);
                north = Random.Range(0, 2);
            }
            Debug.Log(east);
            Debug.Log(west);
            Debug.Log(south);
            Debug.Log(north);
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
                    nodeInfo.north = GenerateNodes(mapInfo, x , y + 1, distance + 1);
            }
        }
        return nodeInfo;
    }

    public int GetRoomsNumber(int distance)
    {
        float probability = 1f / (1f + distance * distance);
        float randomValue = Random.Range(0f, 1f);
        int value = (int)Mathf.Lerp(1f, 4f, randomValue * probability);
        return value;
    }
}
