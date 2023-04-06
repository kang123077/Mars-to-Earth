using Character;
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


    public  void SpawnMonster()
    {
        //인게임 매니저에서 현재 스테이지에 따라 스탯 
        //
    }

    public void Launch(Transform attacker,Vector3 target,int layer, float dmg, Projectile.Mesh mesh,Projectile.Type type)
    {
        Projectile.Projectile projectile = projectileManagedPool.Get();
        projectile.mesh.mesh = ResourceManager.Instance.projectileMesh[(int)mesh].sharedMesh;

        projectile.attacker = attacker;
        projectile.layerMask = (1 << 3 | 1 << 6) ^ 1 << layer;
        projectile.dmg = dmg;
        projectile.targetPosition = target;
        projectile.type = type;
        projectile.gameObject.SetActive(true);
        projectile.Init();
    }

    public static void DropOptanium(Vector3 postion)
    {
        Item.Item optanium= Instantiate(ResourceManager.Instance.items[0],postion,Quaternion.identity);
        Debug.Log("옵타니움"+optanium);
    }
    
}
