using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    private float openDistance;
    private NavMeshObstacle navMeshObstacle;
    public Animator animator;
    public bool isGateOpen;

    private void Start()
    {
        isGateOpen = false;
        openDistance = 6f;
        animator = GetComponent<Animator>();
        navMeshObstacle= GetComponent<NavMeshObstacle>();
    }
    private void Update()
    {
        //DistanceCheck();
        if (isGateOpen)
        {
            GateOpen();
        }
        else
        {
            GateClose();
        }
    }
    public void GateOpen()
    {
        animator.SetBool("isGateOpen", true);
    }
    public void GateClose()
    {
        animator.SetBool("isGateOpen", false);
    }
    public void DistanceCheck()
    {
        float distance = Vector3.Distance(transform.position,
            SpawnManager.Instance.playerTransform.position);
        if (distance <= openDistance)
        {
            isGateOpen = true;
        }
        else
        {
            isGateOpen = false;
        }
    }
    // Gate 애니메이션 이벤트 함수
    // Open의 중간에 실행되는 함수
    public void OnOpen()
    {
        navMeshObstacle.enabled = false;
    }

    // Gate 애니메이션 이벤트 함수
    // Close의 중간에 실행되는 함수
    public void OnClose()
    {
        navMeshObstacle.enabled = true;
    }
}
