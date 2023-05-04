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
        isGateOpen = true;
        navMeshObstacle.enabled = false;
        gameObject.layer = 10;
    }

    public void GateClose()
    {
        animator.SetBool("isGateOpen", false);
        isGateOpen = false;
        navMeshObstacle.enabled = true;
        gameObject.layer = 9;
    }
}