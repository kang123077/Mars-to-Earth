using Character;
using Projectile;
using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : Singleton<SpawnManager>
{
    public Player player;
    [HideInInspector]public Transform playerTransform;
    public IObjectPool<Projectile.Projectile> projectileManagedPool;
    public Projectile.Projectile projectilePrefab;
    public bool playerInstantiateFinished = false;
    public NodeInfo curNode;
    private int _curMonsterCount;

    public int curMonsterCount
    {
        get =>_curMonsterCount;
        set{
            Debug.Log(value);
            if (value == 0)
            {
                curNode.isNodeCleared = true;
                curNode.nodeCollider.enabled = false;
                MapManager.Instance.UpdateGate();
            }
            _curMonsterCount = value;
        }
    }
    public List<Monster> monsterPool;

    protected override void Awake()
    {
        base.Awake();
        player = Instantiate(player);
        playerTransform = player.gameObject.transform;
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
        }
    }


    public void FirstInit()
    {
        curNode = MapManager.nodes[0];
        playerInstantiateFinished = true;

        for(int i = 0; i < 2; i++)
        {
            RandomSpawnMonster(curNode.transform.position);
        }
    }

    public void NodeSpawn(NodeInfo spawnNode)
    {
        curNode = spawnNode;
        for (int i = 0; i < 3; i++)
        {
            RandomSpawnMonster(curNode.transform.position);
        }
    }

    public void RandomSpawnMonster(Vector3 spawnPoint)
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
                RandomSpawnMonster(spawnPoint);
                return;
            }
        }
        //노말 엘리트 
        // Enum 값 배열 생성
        EnemyType[] values = (EnemyType[])Enum.GetValues(typeof(EnemyType));
        // 랜덤 값 선택
        EnemyType randomType = values[UnityEngine.Random.Range(0, values.Length)];
        GetMonster(randomPosition, randomType);

    }    

    public void GetMonster(Vector3 spawnPoint, EnemyType type)
    {
        Character.Character target;
        int findIdx = monsterPool.FindIndex((monster) => monster.enemyType == type);

        if (findIdx < 0)
            target = Instantiate(ResourceManager.Instance.enemys[(int)type],spawnPoint,Quaternion.identity);
        else
        {
            target = monsterPool[findIdx];
            target.transform.position= spawnPoint;
            monsterPool.RemoveAt(findIdx);
            Debug.Log("있어서 가져옴");
        }

        curMonsterCount++;
        target.gameObject.SetActive(true);
    }
    public void ReleaseMonster(Monster target)
    {
        target.gameObject.SetActive(false);
        curMonsterCount--;
        monsterPool.Add(target);
        Debug.Log("반납함");
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
