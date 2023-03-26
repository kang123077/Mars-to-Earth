using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager :Singleton<SpawnManager>
{ 
    public Player player;
    [HideInInspector]public Transform playerTransform;
    protected override void Awake()
    {
        base.Awake();
        player = Instantiate(player);
        playerTransform = player.gameObject.transform;
    }


    
}
