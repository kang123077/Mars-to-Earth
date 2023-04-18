using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void LateUpdate()
    {
        try
        {
            Vector3 direction = (SpawnManager.Instance.playerTransform.position - new Vector3(0, 1f, 0) - transform.position).normalized;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 37f,
                                1 << LayerMask.NameToLayer("Obstacle"));
            Debug.DrawRay(transform.position, direction * 100f, Color.green);
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
