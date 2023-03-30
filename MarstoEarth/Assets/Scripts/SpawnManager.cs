using Character;
using UnityEngine;

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

    public  void spawnMonster()
    {
        //인게임 매니저에서 현재 스테이지에 따라 스탯 
        //
    }

    public static void DropOptanium(Vector3 postion)
    {
        Item.Item optanium= Instantiate(ResourceManager.Instance.items[0],postion,Quaternion.identity);
        Debug.Log("옵타니움"+optanium);
    }
    
}
