using Character;
using Skill;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager :Singleton<SpawnManager>
{ 
    public Player player;
    [HideInInspector]public Transform playerTransform;
    public GameObject projectilePrefab;
    public IObjectPool<Projectile> projectileManagedPool;
    protected override void Awake()
    {
        base.Awake();
        player = Instantiate(player);
        playerTransform = player.gameObject.transform;

        projectileManagedPool = new ObjectPool<Projectile>(() =>
            {
               GameObject copyPrefab= Instantiate(projectilePrefab);
               copyPrefab.SetActive(false);
               return copyPrefab.AddComponent<Projectile>();
            }
                ,
            actionOnRelease: (pt) => pt.gameObject.SetActive(false),defaultCapacity:20,maxSize:40);
    }


    public  void spawnMonster()
    {
        //인게임 매니저에서 현재 스테이지에 따라 스탯 
        //
    }

    public void Launch(Transform attacker,int layer, float dmg,GameObject prefab)
    {
        projectilePrefab = prefab;
        Projectile projectile = projectileManagedPool.Get();
        projectile.attacker = attacker;
        projectile.layerMask = ~(1 << layer)^(1<<8);
        projectile.gameObject.layer = 8;
        projectile.dmg = dmg;
        projectile.gameObject.SetActive(true);
    }

    public static void DropOptanium(Vector3 postion)
    {
        Item.Item optanium= Instantiate(ResourceManager.Instance.items[0],postion,Quaternion.identity);
        Debug.Log("옵타니움"+optanium);
    }
    
}
