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
    public delegate void RoomClearedHandler(NodeInfo thisNode);
    public event RoomClearedHandler OnRoomCleared;

    private bool isRoomCleared = false;
    public bool IsRoomCleared
    {
        get { return isRoomCleared; }
        set
        {
            isRoomCleared = value;
            // isRoomCleared 값이 변경될 때 Pathnode의 OnRoomCleared 이벤트를 실행
            OnRoomCleared?.Invoke(this);
        }
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
