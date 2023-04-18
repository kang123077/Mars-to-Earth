using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public NodeInfo parent;
    public NodeInfo children;
    public GateController gate_1;
    public GateController gate_2;
    private Collider pathCollider;
    private bool roomClearChecker = false;
    private void Awake()
    {
        // 자식 0번에서 GateController 컴포넌트 가져오기
        gate_1 = transform.GetChild(0).GetComponent<GateController>();
        // 자식 1번에서 GateController 컴포넌트 가져오기
        gate_2 = transform.GetChild(1).GetComponent<GateController>();
        pathCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        if (MapManager.Instance.isMapGenerateFinished == true
            && roomClearChecker == false)
        {
            // parent.OnRoomCleared += OnRoomCleared;
            // children.OnRoomCleared += OnRoomCleared;
            roomClearChecker = true;
        }
    }
    public void AddNeighbor(NodeInfo neighbor, Relation relation)
    {
        switch (relation)
        {
            case Relation.Parent:
                parent = neighbor;
                break;
            case Relation.Children:
                children = neighbor;
                break;
        }
    }

    public void UpdateGate()
    {
        // 둘 다 Clear시 collider 끄기 (TriggerEnter와 중복, 수정 예정)
        if (parent.isNodeCleared == true && children.isNodeCleared == true)
        {
            pathCollider.enabled = false;
        }
        OpenClearedGate();
    }

    public void OpenClearedGate()
    {
        if (parent.isNodeCleared == true)
        {
            gate_1.isGateOpen = true;
        }
        if (children.isNodeCleared == true)
        {
            gate_2.isGateOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Collider에 진입 시 다음 방 몬스터 생성하고 문 열어줌, 다시 닫아주진 않는걸로
            MapManager.Instance.CloseAllGate();
            if (parent.isNodeCleared == false)
            {
                SpawnManager.Instance.NodeSpawn(parent);
                gate_1.isGateOpen = true;
                pathCollider.enabled = false;
            }
            if (children.isNodeCleared == false)
            {
                SpawnManager.Instance.NodeSpawn(children);
                gate_2.isGateOpen = true;
                pathCollider.enabled = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // CloseGate();
        }
    }
    public void CloseGate()
    {
        gate_1.isGateOpen = false;
        gate_2.isGateOpen = false;
    }

    private void OnRoomCleared(NodeInfo clearedNode)
    {
        // isRoomCleared 값이 변경 될 때 실행할 코드 작성
        if (clearedNode.isNodeCleared)
        {
            Debug.Log("Room is cleared!");
            if (clearedNode == parent)
            {
                gate_1.isGateOpen = true;
            }
            else
            {
                gate_2.isGateOpen = true;
            }
        }
        else
        {
            Debug.Log("Room is not cleared yet...");
        }
    }
}
public enum Relation
{
    Parent,
    Children,
}
