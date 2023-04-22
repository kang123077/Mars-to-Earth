
using System;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : Singleton<CinemachineManager>
{
    public CinemachineBrain camManager;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera bossCam;
    public Transform follower;
    
    private Vector3 curAngle;
    private float cameraSpeed ;
    void Start()
    {
        playerCam.Follow = follower;
        bossCam.Follow = follower;
        cameraSpeed = 300;
        curAngle = SpawnManager.Instance.player.camPoint.eulerAngles;
    }

    private void Update()
    {
        curAngle.y += Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
        follower.position = SpawnManager.Instance.player.camPoint.position;
        
        follower.rotation = Quaternion.Euler(curAngle);
    }
}
