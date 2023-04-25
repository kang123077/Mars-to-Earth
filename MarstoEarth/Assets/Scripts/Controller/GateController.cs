using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    private NavMeshObstacle navMeshObstacle;
    public Animator animator;
    public bool isGateOpen;

    private void Start()
    {
        isGateOpen = false;
        animator = GetComponent<Animator>();
        navMeshObstacle= GetComponent<NavMeshObstacle>();
    }
    public void GateOpen()
    {
        animator.SetBool("isGateOpen", true);
        navMeshObstacle.enabled = false;
        gameObject.layer = 10;
        Debug.Log("게이트오픈");
    }

    public void GateClose()
    {
        animator.SetBool("isGateOpen", false);
        navMeshObstacle.enabled = true;
        gameObject.layer = 9;
        Debug.Log("게이트클로즈");
    }
}
