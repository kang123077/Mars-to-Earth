using Character;
using Item;
using Projectile;
using Skill;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

enum PoolType
{
    Projectile,
    Effect,
    Monster,
    Item,
}
public class SpawnManager : Singleton<SpawnManager>
{
    public Player player;
    [HideInInspector] public Transform playerTransform;
    public IObjectPool<Projectile.Projectile> projectileManagedPool;
    public Projectile.Projectile projectilePrefab;
    public Item.Item ItemPrefab;
    public ReleaseEffect effectPrefab;
    public bool spawnInstantiateFinished = false;
    public NodeInfo curNode;
    private int _curMonsterCount;
    public int curMonsterCount
    {
        get => _curMonsterCount;
        set
        {
            Debug.Log(value);
            if (value == 0)
            {
                curNode.IsNodeCleared = true;
                InGameManager.Instance.OnRoomCleared();
                Debug.Log("룸 클리어!");
                if (curNode.isBossNode)
                {
                    Debug.Log("보스 클리어!");
                    Debug.Log("게임 클리어!");
                }
            }
            _curMonsterCount = value;
        }
    }
    public List<Monster> monsterPool;
    public List<ReleaseEffect> effectPool;
    public List<Item.Item> itemPool;
    public int curNormal = 3;
    public int curElite = 1;

    public Transform[] objectPool;
    protected override void Awake()
    {
        base.Awake();

        //player = Instantiate(player);
        //playerTransform = player.gameObject.transform;
        projectileManagedPool = new ObjectPool<Projectile.Projectile>(() =>
            {
                Projectile.Projectile copyPrefab = Instantiate(projectilePrefab,objectPool[(int)PoolType.Projectile]);
                copyPrefab.gameObject.SetActive(false);
                return copyPrefab;
            },
            actionOnRelease: (pt) => pt.gameObject.SetActive(false), defaultCapacity: 20, maxSize: 40);
    }

    protected void Update()
    {
        if (MapManager.Instance.isMapGenerateFinished && spawnInstantiateFinished == false)
        {
            FirstInit();
            spawnInstantiateFinished = true;
        }
    }


    public void FirstInit()
    {
        curNode = MapManager.nodes[0];
        player = Instantiate(player);
        playerTransform = player.gameObject.transform;
        NodeSpawn(curNode);
    }

    public void NodeSpawn(NodeInfo spawnNode)
    {
        if (spawnNode.isBossNode)
        {
            RandomSpawnMonster(curNode.transform.position, EnemyPool.Boss);
            // 쫄들을 소환할수도 있긴 함
            return;
        }
        for (int i = 0; i < curNormal; i++)
        {
            RandomSpawnMonster(curNode.transform.position, EnemyPool.Normal);
        }
        for (int i = 0; i < curElite; i++)
        {
            RandomSpawnMonster(curNode.transform.position, EnemyPool.Elite);
        }
    }

    public void BossSpawn(NodeInfo spawnNode)
    {
        curNode = spawnNode;
        // SpawnBoss(curnode.Transform.Position);
    }

    public void RandomSpawnMonster(Vector3 spawnPoint, EnemyPool pool)
    {
        // 랜덤 위치 계산
        float xRange = UnityEngine.Random.Range(-20f, 20f);
        float zRange = UnityEngine.Random.Range(-20f, 20f);
        Vector3 randomPosition = new Vector3(spawnPoint.x + xRange, spawnPoint.y, spawnPoint.z + zRange);
        // 이미 Monster가 있는지 확인
        Collider[] hitColliders = Physics.OverlapSphere(randomPosition, 1f);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Monster")
            {
                // 이미 Monster가 있으면 함수를 다시 실행
                RandomSpawnMonster(spawnPoint, pool);
                return;
            }
        }

        switch (pool)
        {
            case EnemyPool.Normal:
                NormalType[] normals = (NormalType[])Enum.GetValues(typeof(NormalType));
                NormalType normalType = normals[UnityEngine.Random.Range(0, normals.Length)];
                GetMonster(randomPosition, (EnemyType)Enum.Parse(typeof(EnemyType), normalType.ToString()));
                break;
            case EnemyPool.Elite:
                EliteType[] elites = (EliteType[])Enum.GetValues(typeof(EliteType));
                EliteType eliteType = elites[UnityEngine.Random.Range(0, elites.Length)];
                GetMonster(randomPosition, (EnemyType)Enum.Parse(typeof(EnemyType), eliteType.ToString()));
                break;
            case EnemyPool.Boss:
                BossType[] bosses = (BossType[])Enum.GetValues(typeof(BossType));
                BossType bossType = bosses[UnityEngine.Random.Range(0, bosses.Length)];
                GetMonster(randomPosition, (EnemyType)Enum.Parse(typeof(EnemyType), bossType.ToString()));
                Debug.Log(bossType.ToString());
                break;
            default:
                break;
        }
    }

    public void GetMonster(Vector3 spawnPoint, EnemyType type)
    {
        Character.Character target;
        int findIdx = monsterPool.FindIndex((monster) => monster.enemyType == type);

        if (findIdx < 0)
            target = Instantiate(ResourceManager.Instance.enemys[(int)type], spawnPoint, Quaternion.identity, objectPool[(int)PoolType.Monster]);
        else
        {
            target = monsterPool[findIdx];
            target.transform.position = spawnPoint;
            monsterPool.RemoveAt(findIdx);
        }

        curMonsterCount++;
        target.gameObject.SetActive(true);
    }

    public void ReleaseMonster(Monster target)
    {
        target.gameObject.SetActive(false);
        curMonsterCount--;
        monsterPool.Add(target);
    }
    public ReleaseEffect GetEffect(Vector3 spawnPoint, ParticleSystem particle, float duration=-1,Vector3 scale = default)
    {
        ReleaseEffect target;
        int findIdx = effectPool.FindIndex((el) => ReferenceEquals(el.refParticle, particle));
        
        if (findIdx < 0)
        {
            ParticleSystem targetParticle = Instantiate(particle, spawnPoint, Quaternion.identity,objectPool[(int)PoolType.Effect]);
            ParticleSystem.MainModule main = targetParticle.main;
            main.stopAction = ParticleSystemStopAction.Callback;

            target = targetParticle.AddComponent<ReleaseEffect>();
            target.refParticle = particle;
            target.gameObject.SetActive(false);
        }
        else
        {
            target = effectPool[findIdx];
            target.transform.position = spawnPoint;
            effectPool.RemoveAt(findIdx);
        }

        if (scale == default)
            scale = Vector3.one;

        target.Init(duration,scale);
        target.gameObject.SetActive(true);
        return target;
    }



    public void Launch(Vector3 ap, Vector3 tp, float dg, float dr, float sp, float rg, ref ProjectileInfo info)
    {
        Projectile.Projectile projectile = projectileManagedPool.Get();
        projectile.Init(ap, tp, dg, dr, sp, rg, ref info);
        projectile.gameObject.SetActive(true);
    }

    private int[] weight = { 3, 7, 11 };
    public void DropItem(Vector3 spawnPoint, EnemyPool rank)
    {
        if(weight[(int)rank]<UnityEngine.Random.Range(1, 11))
            return;
        Item.Item target;

        ItemType type = (ItemType)UnityEngine.Random.Range(0, 3);
        int findIdx = itemPool.FindIndex((el) => el.type == type);

        if (findIdx < 0)
        {
            target = Instantiate(ItemPrefab, spawnPoint, Quaternion.identity,objectPool[(int)PoolType.Item]);
            Instantiate(ResourceManager.Instance.itemInfos[(int)type].thisParticle, target.transform);
            target.type = type;

        }
        else
        {
            target = itemPool[findIdx];
            target.transform.position = spawnPoint;
            itemPool.RemoveAt(findIdx);
        }

        target.gameObject.SetActive(true);
    }
}