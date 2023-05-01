using UnityEngine;

public class OuterController : MonoBehaviour
{
    public PathController pathController;

    public void OnTriggerExit(Collider other)
    {
        if (pathController.isColliderIn)
        {
            Debug.Log("outerExit");
            pathController.ExitEvent(other);
            pathController.isColliderIn = false;
        }
    }
}
