using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour
{
    public int x;
    public int y;
    public int distance;
    public NodeInfo east;
    public NodeInfo west;
    public NodeInfo north;
    public NodeInfo south;
    public bool isBossNode;
    public BoxCollider nodeCollider;
    public List<MeshRenderer> meshRenderers;
    private bool nodeInitFinished;


    public delegate void RoomClearedHandler(NodeInfo clearedNode);
    public event RoomClearedHandler OnRoomCleared;
    public bool isNodeCleared;
    public bool IsNodeCleared
    {
        get { return isNodeCleared; }
        set
        {
            isNodeCleared = value;
            if (OnRoomCleared != null)
            {
                OnRoomCleared(this);
            }
        }
    }

    public delegate void RoomRenderedHandler();
    public event RoomRenderedHandler OnRoomRendered;
    public bool isNodeRendered;
    public bool IsNodeRendered
    {
        get { return isNodeRendered; }
        set
        {
            isNodeRendered = value;
            if (OnRoomRendered != null)
            {
                OnRoomRendered();
            }
        }
    }

    private void Awake()
    {
        nodeCollider = GetComponent<BoxCollider>();
        meshRenderers = new List<MeshRenderer>();
        isBossNode = false;
        nodeInitFinished = false;
    }

    private void Update()
    {
        if (SpawnManager.Instance.spawnInstantiateFinished && !nodeInitFinished)
        {
            CollectMeshRenderers(transform);
            if (SpawnManager.Instance.curNode != this)
            {
                SetMeshRendererEnabled(false);
                IsNodeRendered = false;
            }
            nodeInitFinished = true;
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

        IsNodeRendered = isEnabled;
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].enabled = isEnabled;
        }
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
