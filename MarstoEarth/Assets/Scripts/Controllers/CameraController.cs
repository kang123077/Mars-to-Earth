using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void LateUpdate()
    {
        try
        {
            Vector3 direction = (SpawnManager.Instance.playerTransform.position - new Vector3(0, -1f, 0) - transform.position).normalized;
            Vector3 startingPosition = transform.position - direction * 4f;
            float distance = Vector3.Distance(startingPosition, SpawnManager.Instance.playerTransform.position);
            RaycastHit[] hits = Physics.RaycastAll(startingPosition, direction, distance,
                                1 << LayerMask.NameToLayer("Obstacle"));
            Debug.DrawRay(startingPosition, direction * distance, Color.green);
            for (int i = 0; i < hits.Length; i++)
            {
                ObstacleController[] obj = hits[i].transform.GetComponentsInChildren<ObstacleController>();

                for (int j = 0; j < obj.Length; j++)
                {
                    obj[j]?.BecomeTransparent();
                }
            }
        }
        catch(MissingReferenceException)
        {
            // 케릭터 죽은 후 에러
        }

    }
}
