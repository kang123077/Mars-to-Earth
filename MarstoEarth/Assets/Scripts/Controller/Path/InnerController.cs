using UnityEngine;

public class InnerController : MonoBehaviour
{
    public PathController pathController;

    public void OnTriggerEnter(Collider other)
    {
        if (!pathController.isColliderIn)
        {
            Debug.Log("innerEnter");
            pathController.EnterEvent(other);
            pathController.isColliderIn = true;
        }
    }
}
