using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void LateUpdate()
    {
        // Player는 싱글톤이기에 전역적으로 접근할 수 있습니다.
        Vector3 direction = (SpawnManager.playerTransform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity,
                            1 << LayerMask.NameToLayer("Obstacle"));

        for (int i = 0; i < hits.Length; i++)
        {
            ObstacleController[] obj = hits[i].transform.GetComponentsInChildren<ObstacleController>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }
}
