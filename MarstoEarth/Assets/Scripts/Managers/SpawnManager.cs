using Character;
using Projectile;
using Skill;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : Singleton<SpawnManager>
{
    public Player player;
    [HideInInspector] public Transform playerTransform;
    public IObjectPool<Projectile.Projectile> projectileManagedPool;
    public Projectile.Projectile projectilePrefab;

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

    public void RandomSpawnMonster(Vector3 spawnPoint)
    {
        // 랜덤 위치 계산
        float xRange = UnityEngine.Random.Range(-26f, 26f);
        float zRange = UnityEngine.Random.Range(-26f, 26f);
        Vector3 randomPosition = new Vector3(spawnPoint.x + xRange, spawnPoint.y, spawnPoint.z + zRange);
        // 해당 위치에 이미 몬스터가 있는지 검사
        Collider[] hitColliders = Physics.OverlapBox(randomPosition, new Vector3(2f, 2f, 2f), Quaternion.identity);
        if (hitColliders.Length > 0)
        {
            // 해당 위치에 이미 몬스터가 있으면 다시 함수를 실행하여 다른 위치에 몬스터를 생성
            RandomSpawnMonster(spawnPoint);
        }
        else
        {
            // Enum 값 배열 생성
            EnemyType[] values = (EnemyType[])Enum.GetValues(typeof(EnemyType));
            // 랜덤 값 선택
            EnemyType randomType = values[UnityEngine.Random.Range(0, values.Length)];
            // 해당 위치에 몬스터 인스턴스화
            Instantiate(ResourceManager.Instance.enemys[(int)randomType], randomPosition, Quaternion.identity);
        }
    }

    public void SpawnMonster(Vector3 spawnPoint, EnemyType type)
    {
        Instantiate(ResourceManager.Instance.enemys[(int)type], spawnPoint, Quaternion.identity);
    }


    public void Launch(Vector3 ap, Vector3 tp, float dg, float dr, float sp, float rg, ref ProjectileInfo info)
    {
        Projectile.Projectile projectile = projectileManagedPool.Get();
        projectile.Init(ap, tp, dg, dr, sp, rg, ref info);
        projectile.gameObject.SetActive(true);
    }

    public static void DropOptanium(Vector3 postion)
    {
        Item.Item optanium = Instantiate(ResourceManager.Instance.items[0], postion, Quaternion.identity);
    }

}
