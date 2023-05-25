using UnityEngine;

public class InnerController : MonoBehaviour
{
    public PathController pathController;

    public void OnTriggerEnter(Collider other)
    {
        if (pathController.isColliderOn)
        {
            pathController.EnterEvent(other);
            gameObject.SetActive(false);
            pathController.isColliderOn = false;
        }
    }
}
