using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    private NavMeshObstacle navMeshObstacle;
    private AudioSource audioSource;
    public Animator animator;
    public bool isGateOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshObstacle= GetComponent<NavMeshObstacle>();
        audioSource = GetComponent<AudioSource>();
        isGateOpen = false;
    }

    public void GateOpen()
    {
        animator.SetBool("isGateOpen", true);
        AudioManager.Instance.PlayEffect((int)CombatEffectClip.steam, audioSource);
        isGateOpen = true;
        gameObject.layer = 10;
        navMeshObstacle.enabled = false;
    }

    public void GateClose()
    {
        animator.SetBool("isGateOpen", false);
        AudioManager.Instance.PlayEffect((int)CombatEffectClip.steam, audioSource);
        isGateOpen = false;
        gameObject.layer = 9;
        navMeshObstacle.enabled = true;
    }
}
