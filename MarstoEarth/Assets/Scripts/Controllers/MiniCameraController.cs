using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCameraController : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {
        if (SpawnManager.Instance.spawnInstantiateFinished)
        {
            transform.position = new Vector3(SpawnManager.Instance.playerTransform.position.x, transform.position.y, SpawnManager.Instance.playerTransform.position.z);
        }
    }
}
