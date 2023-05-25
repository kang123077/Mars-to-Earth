using UnityEngine;

public class OuterController : MonoBehaviour
{
    public PathController pathController;

    private void OnTriggerEnter(Collider other)
    {
        SpawnManager.Instance.player.isInsidePath = true;
        SpawnManager.Instance.player.target = null;
    }

    public void OnTriggerExit(Collider other)
    {
        SpawnManager.Instance.player.isInsidePath = false;
        if (!pathController.isColliderOn)
        {
            pathController.ExitEvent(other);
            pathController.isColliderOn = true;
        }
    }
}
