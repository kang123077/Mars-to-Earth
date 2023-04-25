using Character;
using Projectile;
using Skill;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : Singleton<SpawnManager>
{
    public Player player;
    [HideInInspector] public Transform playerTransform;
    public IObjectPool<Projectile.Projectile> projectileManagedPool;
    public Projectile.Projectile projectilePrefab;
    public bool playerInstantiateFinished = false;
    private NodeInfo curNode;
    public List<Monster> monsters;

    public int curNormal = 3;
    public int curElite = 1;

    protected override void Awake()
    {
        base.Awake();


        projectileManagedPool = new ObjectPool<Projectile.Projectile>(() =>
            {
                Projectile.Projectile copyPrefab = Instantiate(projectilePrefab);
                copyPrefab.gameObject.SetActive(false);
                return copyPrefab;
            },
            actionOnRelease: (pt) => pt.gameObject.SetActive(false), defaultCapacity: 20, maxSize: 40);
    }

    protected void Update()
    {
        if (MapManager.Instance.isMapGenerateFinished && playerInstantiateFinished == false)
        {
            FirstInit();
            playerInstantiateFinished = true;
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
        curNode = spawnNode;
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
                MonsterObjectPooling(randomPosition, (EnemyType)Enum.Parse(typeof(EnemyType), normalType.ToString()));
                break;
            case EnemyPool.Elite:
                EliteType[] elites = (EliteType[])Enum.GetValues(typeof(EliteType));
                EliteType eliteType = elites[UnityEngine.Random.Range(0, elites.Length)];
                MonsterObjectPooling(randomPosition, (EnemyType)Enum.Parse(typeof(EnemyType), eliteType.ToString()));
                break;
            case EnemyPool.Boss:
                BossType[] bosses = (BossType[])Enum.GetValues(typeof(BossType));
                BossType bossType = bosses[UnityEngine.Random.Range(0, bosses.Length)];
                MonsterObjectPooling(randomPosition, (EnemyType)Enum.Parse(typeof(EnemyType), bossType.ToString()));
                break;
            default:
                break;
        }
    }

    public void MonsterObjectPooling(Vector3 spawnPoint, EnemyType type)
    {
        if (monsters.Count == 0)
        {
            SpawnMonster(spawnPoint, type);
            return;
        }
        foreach (Monster monster in monsters)
        {
            if (monster.enemyType == type && !monster.gameObject.activeSelf)
            {
                monster.gameObject.SetActive(true);
                return;
            }
            else
            {
                SpawnMonster(spawnPoint, type);
                return;
            }
        }
    }
    public void SpawnMonster(Vector3 spawnPoint, EnemyType type)
    {
        Monster newMonster = Instantiate(ResourceManager.Instance.enemys[(int)type], spawnPoint, Quaternion.identity).GetComponent<Monster>();
        monsters.Add(newMonster);
    }

    public void ClearCheck()
    {
        if (monsters.Count == 0)
        {
            Debug.Log("룸 클리어!");
            curNode.IsNodeCleared = true;
            InGameManager.Instance.OnRoomCleared();
            if (curNode.isBossNode)
            {
                Debug.Log("보스 클리어!");
            }
            // MapManager.Instance.UpdateGate();
        }
    }

    public void Launch(Vector3 ap, Vector3 tp, float dg, float dr, float sp, float rg, ref ProjectileInfo info)
    {
        Projectile.Projectile projectile = projectileManagedPool.Get();
        projectile.Init(ap, tp, dg, dr, sp, rg, ref info);
        projectile.gameObject.SetActive(true);
    }

    public static void DropOptanium(Vector3 postion)
    {
        Instantiate(ResourceManager.Instance.items[0], postion, Quaternion.identity);

    }

}
