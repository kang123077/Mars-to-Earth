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
