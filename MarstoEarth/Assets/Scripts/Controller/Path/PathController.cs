using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public NodeInfo parent;
    public NodeInfo children;
    public bool isColliderOn;
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
        isColliderOn = true;
    }
    private void Update()
    {
        if (MapManager.Instance.isMapGenerateFinished == true
            && roomClearChecker == false)
        {
            // Delegate 구독
            parent.OnRoomCleared += OnRoomCleared;
            children.OnRoomCleared += OnRoomCleared;
            parent.OnRoomRendered += CheckNeighborNode;
            children.OnRoomRendered += CheckNeighborNode;
            CheckNeighborNode();
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

    public void CheckNeighborNode()
    {
        if (SpawnManager.Instance.curNode == parent ||
            SpawnManager.Instance.curNode == children)
        {
            SetMeshRendererEnabled(true);
        }
        else
        {
            SetMeshRendererEnabled(false);
            CloseGate();
        }
    }

    public void SetMeshRendererEnabled(bool isEnabled)
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
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
        if (parent.IsNodeCleared)
        {
            gate_1.GateOpen();
        }
        if (children.IsNodeCleared)
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
                SpawnManager.Instance.curNode = children;
                children.IsNodeRendered = true;
                gate_2.GateOpen();
                Invoke("SetParentMeshFalse", 2.0f);
            }
            // gate_2(children방향)으로 진입
            else
            {
                gate_2.GateClose();
                SpawnManager.Instance.curNode = parent;
                parent.IsNodeRendered = true;
                gate_1.GateOpen();
                Invoke("SetChildrenMeshFalse", 2.0f);
            }
        }
    }

    public void SetChildrenMeshFalse()
    {
        children.IsNodeRendered = false;
        if (parent.IsNodeCleared)
        {
            // 이미 깬 곳이면 바로 true 줘서 주변 문 열리도록
            parent.IsNodeCleared = true;
        }
    }
    public void SetParentMeshFalse()
    {
        parent.IsNodeRendered = false;
        if (children.IsNodeCleared)
        {
            // 이미 깬 곳이면 바로 true 줘서 주변 문 열리도록
            children.IsNodeCleared = true;
        }
    }

    public void ExitEvent(Collider other)
    {
    }

    private void OnRoomCleared(NodeInfo clearedNode)
    {
        innerCollider.gameObject.SetActive(true);
        if (clearedNode.IsNodeCleared)
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
