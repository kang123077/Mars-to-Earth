using UnityEngine;

public class GateController : MonoBehaviour
{
    private float openDistance;
    public Animator animator;
    public bool isGateOpen;

    private void Start()
    {
        isGateOpen = false;
        openDistance = 5f;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        DistanceCheck();
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
    public void OnOpenEnd()
    {
        // Gate 애니메이션이 종료되면 NavMesh를 업데이트
        // MapManager.Instance.ResetNavMesh();
    }

    // Gate 애니메이션 이벤트 함수
    public void OnCloseEnd()
    {
        // Gate 애니메이션이 종료되면 NavMesh를 업데이트
        // MapManager.Instance.ResetNavMesh();
    }
}
