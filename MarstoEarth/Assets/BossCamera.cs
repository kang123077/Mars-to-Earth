using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : Singleton<BossCamera>
{
    public Cinemachine.CinemachineVirtualCamera vcam;
    public float TurnSpeed = 2f;
    // Start is called before the first frame update
    protected void Start()
    {
        vcam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        vcam.Follow = SpawnManager.Instance.player.cameraView;
    }
    void Update()
    {
        
    }
}
