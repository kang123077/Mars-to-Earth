using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public NodeGenerator nodeGenerator;

    private bool isNodeSet;
    public void GenerateMap()
    {
        if (isNodeSet == false)
        {
            nodeGenerator.GenerateNodes(0, 0, 0, null, 0);
            isNodeSet = true;
        }
    }
    public void NodeClear()
    {
        isNodeSet = false;
    }

    public void DestroyGenerators()
    {
        Destroy(nodeGenerator.gameObject);
        Destroy(gameObject);
    }
}