using Character;
using Projectile;
using Skill;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager :Singleton<SpawnManager>
{ 
    public Player player;
    [HideInInspector]public Transform playerTransform;
    public IObjectPool<Projectile.Projectile> projectileManagedPool;
    public Projectile.Projectile projectilePrefab;

    protected override void Awake()
    {
        base.Awake();
        player = Instantiate(player);
        playerTransform = player.gameObject.transform;

        projectileManagedPool = new ObjectPool<Projectile.Projectile>(() =>
            {
               Projectile.Projectile copyPrefab=Instantiate(projectilePrefab);
               copyPrefab.gameObject.SetActive(false);
               return copyPrefab;
            },
            actionOnRelease: (pt) => pt.gameObject.SetActive(false),defaultCapacity:20,maxSize:40);
    }

    public  void SpawnMonster(Vector3 spawnPoint, EnemyType type)
    {
        Instantiate(ResourceManager.Instance.enemys[(int)type], spawnPoint, Quaternion.identity);
    }

    public void Launch(Vector3 ap, Vector3 tp, float dg, float dr, float sp, float rg, ref ProjectileInfo info)
    {
        Projectile.Projectile projectile = projectileManagedPool.Get();
        projectile.Init(ap,tp,dg,dr,sp,rg,ref info);
        projectile.gameObject.SetActive(true);
    }

    public static void DropOptanium(Vector3 postion)
    {
        Instantiate(ResourceManager.Instance.items[0],postion,Quaternion.identity);
    }
    
}
