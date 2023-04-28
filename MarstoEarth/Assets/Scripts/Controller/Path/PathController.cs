using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public NodeInfo parent;
    public NodeInfo children;
    public bool isColliderIn = false;
    public Collider innerCollider;
    private GateController gate_1;
    private GateController gate_2;
    private bool roomClearChecker = false;

    private List<MeshRenderer> meshRenderers;
    private void Awake()
    {
        gate_1 = transform.GetChild(0).GetComponent<GateController>();
        gate_2 = transform.GetChild(1).GetComponent<GateController>();
        meshRenderers = new List<MeshRenderer>();
        CollectMeshRenderers(transform);
    }
    private void Update()
    {
        if (MapManager.Instance.isMapGenerateFinished == true
            && roomClearChecker == false)
        {
            // Delegate 구독
            parent.OnRoomCleared += OnRoomCleared;
            children.OnRoomCleared += OnRoomCleared;
            parent.OnRoomRendered += OnRoomRendered;
            children.OnRoomRendered += OnRoomRendered;
            roomClearChecker = true;
        }
    }

    private void CollectMeshRenderers(Transform transform)
    {
        foreach (Transform child in transform)
        {
            // Check if the child object has the desired script attached 11 = Minimap
            if (child.GetComponent(typeof(MeshRenderer)) != null && child.gameObject.layer != 11)
            {
                // If the child has the script attached, add its MeshRenderer to the list
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderers.Add(meshRenderer);
                }
            }

            // Recursively call the CollectMeshRenderers function on each child object
            CollectMeshRenderers(child);
        }
    }
    public void SetMeshRendererEnabled(bool isEnabled)
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            Debug.Log("false하라고");
            meshRenderers[i].enabled = isEnabled;
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

    public void EnterEvent(Collider other)
    {
        if (other.tag == "Player")
        {
            // Collider에 진입 시 다음 방 몬스터 생성하고 문 열어줌, 다시 닫아주진 않는걸로
            // gate_1(parent방향)으로 진입
            if (gate_1.isGateOpen)
            {
                gate_1.GateClose();
                if (!children.isNodeCleared)
                {
                    SpawnManager.Instance.NodeSpawn(children);
                    innerCollider.gameObject.SetActive(false);
                }
                gate_2.GateOpen();
            }
            // gate_2(children방향)으로 진입
            else
            {
                gate_2.GateClose();
                if (!parent.IsNodeCleared)
                {
                    SpawnManager.Instance.NodeSpawn(parent);
                    innerCollider.gameObject.SetActive(false);
                }
                gate_1.GateOpen();
            }
        }
    }

    public void ExitEvent(Collider other)
    {
        if (parent.isNodeCleared && children.isNodeCleared && other.tag == "Player")
        {
            // CloseGate();
        }
    }

    private void OnRoomCleared(NodeInfo clearedNode)
    {
        innerCollider.gameObject.SetActive(true);
        if (clearedNode.isNodeCleared)
        {
            if (clearedNode == parent)
            {
                gate_1.GateOpen();
            }
            else
            {
                gate_2.GateOpen();
            }
        }
    }

    private void OnRoomRendered(bool value)
    {
        SetMeshRendererEnabled(value);
    }

    public void CloseGate()
    {
        gate_1.GateClose();
        gate_2.GateClose();
    }
}

public enum Relation
{
    // 노드 생성 시 부모 방향 (gate_1)
    Parent,
    // 노드 생성 시 자식 방향 (gate_2)
    Children,
}
