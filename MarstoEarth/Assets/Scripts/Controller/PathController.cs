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
    public Collider pathCollider;
    private void Awake()
    {
        // 자식 0번에서 GateController 컴포넌트 가져오기
        gate_1 = transform.GetChild(0).GetComponent<GateController>();
        // 자식 1번에서 GateController 컴포넌트 가져오기
        gate_2 = transform.GetChild(1).GetComponent<GateController>();
        pathCollider = GetComponent<Collider>();
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("감지성공");
        }
    }
}
public enum Relation
{
    Parent,
    Children,
}
