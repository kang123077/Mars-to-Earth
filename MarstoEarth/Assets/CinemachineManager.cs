
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
        Vector2 rotInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        curAngle.y += rotInput.x * cameraSpeed * Time.deltaTime;
        follower.position = SpawnManager.Instance.player.camPoint.position;
        
        follower.rotation = Quaternion.Euler(curAngle);
    }
}
