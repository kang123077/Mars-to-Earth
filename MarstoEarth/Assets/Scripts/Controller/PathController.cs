using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public NodeInfo parent;
    public NodeInfo children;
    private GateController gate_1;
    private GateController gate_2;
    private Collider pathCollider;
    private bool roomClearChecker = false;
    private void Awake()
    {
        gate_1 = transform.GetChild(0).GetComponent<GateController>();
        gate_2 = transform.GetChild(1).GetComponent<GateController>();
        pathCollider = GetComponent<Collider>();
        // Delegate 구독
    }
    private void Update()
    {
        if (MapManager.Instance.isMapGenerateFinished == true
            && roomClearChecker == false)
        {
            parent.OnRoomCleared += OnRoomCleared;
            children.OnRoomCleared += OnRoomCleared;
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
        OpenClearedGate();
    }

    public void OpenClearedGate()
    {
        if (parent.isNodeCleared == true)
        {
            gate_1.GateOpen();
        }
        if (children.isNodeCleared == true)
        {
            gate_2.GateOpen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Collider에 진입 시 다음 방 몬스터 생성하고 문 열어줌, 다시 닫아주진 않는걸로
            // MapManager.Instance.CloseAllGate();
            gate_1.GateClose();
            gate_2.GateClose();
            if (parent.isNodeCleared == false)
            {
                SpawnManager.Instance.NodeSpawn(parent);
                gate_1.GateOpen();
                pathCollider.enabled = false;
            }
            if (children.isNodeCleared == false)
            {
                SpawnManager.Instance.NodeSpawn(children);
                gate_2.GateOpen();
                pathCollider.enabled = false;
            }
        }
    }
    private void OnRoomCleared(NodeInfo clearedNode)
    {
        if (clearedNode.isNodeCleared)
        {
            if (clearedNode == parent)
            {
                gate_1.GateOpen();
                if (children.isNodeCleared)
                {
                    gate_2.GateOpen();
                }
            }
            else
            {
                gate_2.GateOpen();
                if (parent.isNodeCleared)
                {
                    gate_1.GateOpen();
                }
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
}

public enum Relation
{
    Parent,
    Children,
}
