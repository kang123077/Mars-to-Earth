using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeInfo : MonoBehaviour
{
    public int x;
    public int y;
    public NodeInfo east;
    public NodeInfo west;
    public NodeInfo north;
    public NodeInfo south;
    public bool isNodeCleared;
    public BoxCollider nodeCollider;

    public delegate void RoomClearedHandler(NodeInfo clearedNode);
    public event RoomClearedHandler OnRoomCleared;

    public bool IsNodeCleared
    {
        get { return isNodeCleared; }
        set
        {
            isNodeCleared = value;
            Debug.Log("setNodeCleared");
            if (OnRoomCleared != null)
            {
                OnRoomCleared(this);
            }
        }
    }

    private void Awake()
    {
        nodeCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // MapManager.Instance.CloseAllGate();
    }
    public NodeInfo(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void AddNeighbor(NodeInfo neighbor, Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                east = neighbor;
                break;
            case Direction.West:
                west = neighbor;
                break;
            case Direction.North:
                north = neighbor;
                break;
            case Direction.South:
                south = neighbor;
                break;
        }
    }
}
public enum Direction
{
    East,
    West,
    North,
    South
}
