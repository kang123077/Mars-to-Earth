using UnityEngine;

public class OuterController : MonoBehaviour
{
    public PathController pathController;

    public void OnTriggerExit(Collider other)
    {
        if (!pathController.isColliderOn)
        {
            pathController.ExitEvent(other);
            pathController.isColliderOn = true;
        }
    }
}
