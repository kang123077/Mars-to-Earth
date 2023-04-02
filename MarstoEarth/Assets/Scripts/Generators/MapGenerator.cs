using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public NodeGenerator nodeGenerator;
    public void GenerateMap(MapInfo mapInfo)
    {
        nodeGenerator.GenerateNodes(mapInfo, 0, 0, 0);
    }
}