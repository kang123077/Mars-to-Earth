// using UnityEditor.UIElements;
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
        isGateOpen = true;
        gameObject.layer = 10;
        navMeshObstacle.enabled = false;
    }

    public void GateClose()
    {
        animator.SetBool("isGateOpen", false);
        isGateOpen = false;
        gameObject.layer = 9;
        navMeshObstacle.enabled = true;
    }
}
