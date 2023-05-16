using System;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : Singleton<CinemachineManager>
{
    public CinemachineBrain camManager;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera bossCam;
    public Transform follower;

    public Vector3 curAngle;
    public float cameraSpeed ;
    void Start()
    {
        playerCam.Follow = follower;
        bossCam.Follow = follower;
        cameraSpeed = 300;
        curAngle = SpawnManager.Instance.player.camPoint.eulerAngles;
        follower.position = SpawnManager.Instance.player.camPoint.position;
        follower.rotation = Quaternion.Euler(curAngle);
    }

    private void Update()
    {
        follower.position = SpawnManager.Instance.player.camPoint.position;


#if UNITY_STANDALONE_WIN
        curAngle.y += Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
        follower.rotation = Quaternion.Euler(curAngle);
#endif
    }

}
