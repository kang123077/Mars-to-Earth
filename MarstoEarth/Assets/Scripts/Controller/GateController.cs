using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public Animator animator;
    public bool isGateOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isGateOpen)
        {
            animator.SetBool("isGateOpen", true);
        }
        else
        {
            animator.SetBool("isGateOpen", false);
        }
    }
}
