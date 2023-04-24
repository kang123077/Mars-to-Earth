using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : Singleton<CinemachineManager>
{
    public CinemachineBrain camManager;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera bossCam;

    void Start()
    {
    }

    void Update()
    {
        if (SpawnManager.Instance.playerInstantiateFinished)
        {
            playerCam.Follow = SpawnManager.Instance.player.cameraView;
            bossCam.Follow = SpawnManager.Instance.player.cameraView;
        }

    }
}
