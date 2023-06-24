using Character;
using Item;
using Projectile;
using Skill;
using System;
using System.Collections;
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
    public Item.UsingItem ItemPrefab;
    public Item.StoryItem StoryItemPrefab;
    public List<Monster> monsterPool;
    public bool spawnInstantiateFinished;
    public LightController outsideLight;
    public LightController insideLight;
    private int _curMonsterCount;
    private int curNormal = 3;
    private int curElite = 1;
    private int curBoss = 1;

    public int curMonsterCount
    {
        get => _curMonsterCount;
        set
        {
            if (value == 0)
            {
                curNode.IsNodeCleared = true;
                if (curNode.isBossNode)
                {
                    Invoke("RequestStageClear", 6f);
                    InGameManager.Instance.OnBossCleared();
                }
                else
                {
                    InGameManager.Instance.OnRoomCleared();
                }
            }
            _curMonsterCount = value;
        }
    }
    public List<ReleaseEffect> effectPool;
    public List<Item.UsingItem> itemPool;

    private NodeInfo _curNode;
    public NodeInfo curNode
    {
        get { return _curNode; }
        set
        {
            _curNode = value;
            curNode.alertIcon.gameObject.SetActive(false);
            StartCoroutine(CheckLightCoroutine(_curNode.isInside));
            if (!curNode.IsNodeCleared)
            {
                NodeSpawn(curNode);
            }
        }
    }

    public Transform[] objectPool;
    public AudioSource effectSound;
    protected override void Awake()
    {
        base.Awake();
        spawnInstantiateFinished = false;
        projectileManagedPool = new ObjectPool<Projectile.Projectile>(() =>
            {
                Projectile.Projectile copyPrefab = Instantiate(projectilePrefab, objectPool[(int)PoolType.Projectile]);
                copyPrefab.gameObject.SetActive(false);
                return copyPrefab;
            },
            actionOnRelease: (pt) => pt.gameObject.SetActive(false), defaultCapacity: 20, maxSize: 40);

        /*
         * 스토리 도감 로딩-> 
         */
    }

    public void RequestStageClear()
    {
        staticStat.saveStat(player);
        UIManager.Instance.StageClear();
    }

    public void InitSpawn()
    {
        curNode = MapManager.Instance.nodes[0];
        CheckLight(curNode.isInside);
        player = Instantiate(player);
        playerTransform = player.gameObject.transform;
        spawnInstantiateFinished = true;
    }

    public void NodeSpawn(NodeInfo spawnNode)
    {
        curDiffcultyCheck();
        if (spawnNode.isBossNode)
        {
            for (int i = 0; i < curBoss; i++)
            {
                RandomSpawnMonster(curNode.transform.position, EnemyPool.Boss);
            }
            return;
        }
        for (int i = 0; i < curElite; i++)
        {
            RandomSpawnMonster(curNode.transform.position, EnemyPool.Elite);
        }
        for (int i = 0; i < curNormal; i++)
        {
            RandomSpawnMonster(curNode.transform.position, EnemyPool.Normal);
        }
    }

    public void curDiffcultyCheck()
    {
        int curDifficulty = Mathf.FloorToInt(MapInfo.difficulty);
        if (curDifficulty > 2)
        {
            curNormal = 4;
        }
        if (curDifficulty > 4)
        {
            curNormal = 5;
        }
        if (curDifficulty > 6)
        {
            curElite = 2;
        }
        if (curDifficulty > 8)
        {
            curNormal = 6;
            curBoss = 2;
        }
        if (curDifficulty > 10)
        {
            curNormal = 7;
        }
        if (curDifficulty > 12)
        {
            curNormal = 8;
        }
        if (curDifficulty > 14)
        {
            curElite = 3;
        }
        if (curDifficulty > 16)
        {
            curBoss = 3;
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
    public ReleaseEffect GetEffect(Vector3 spawnPoint, ParticleSystem particle, int clipNum, float scale = 1, float duration = -1)
    {
        ReleaseEffect target;
        int findIdx = effectPool.FindIndex((el) => ReferenceEquals(el.refParticle, particle));

        if (findIdx < 0)
        {
            ParticleSystem targetParticle = Instantiate(particle, spawnPoint, Quaternion.identity, objectPool[(int)PoolType.Effect]);
            ParticleSystem.MainModule main = targetParticle.main;

            main.stopAction = ParticleSystemStopAction.Callback;
            targetParticle.gameObject.SetActive(false);
            target = targetParticle.AddComponent<ReleaseEffect>();

            target.sound = Instantiate(effectSound, target.transform);

            target.refParticle = particle;
        }
        else
        {
            target = effectPool[findIdx];
            target.transform.position = spawnPoint;
            effectPool.RemoveAt(findIdx);
        }

        target.Init(duration, scale);
        AudioManager.Instance.PlayEffect(clipNum, target.sound);
        target.gameObject.SetActive(true);
        return target;
    }

    public void Launch(Vector3 ap, Vector3 tp, float dg, float dr, float sp, float rg, ref ProjectileInfo info)
    {
        Projectile.Projectile projectile = projectileManagedPool.Get();
        projectile.Init(ap, tp, dg, dr, sp, rg, ref info);
        projectile.gameObject.SetActive(true);
    }

    private int[] weight = { 2, 6, 10 };
    public static System.Random rand = new();

    public IEnumerator CheckLightCoroutine(bool isInside)
    {
        if (isInside)
        {
            insideLight.addMonsterLayer();
            insideLight.gameObject.SetActive(true);
            outsideLight.removeMonsterLayer();
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            outsideLight.gameObject.SetActive(false);
        }
        else
        {
            outsideLight.addMonsterLayer();
            outsideLight.gameObject.SetActive(true);
            insideLight.removeMonsterLayer();
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            insideLight.gameObject.SetActive(false);
        }
    }

    public void CheckLight(bool isInside)
    {
        insideLight.gameObject.SetActive(isInside);
        outsideLight.gameObject.SetActive(!isInside);
    }

    public void DropItem(Vector3 spawnPoint, EnemyPool rank)
    {
        // int randValue = rand.Next(0, 200);
        int randValue = 199;
        // storyItem
        if (randValue == 199 && MapInfo.storyValue < 3)
        {
            DropStoryItem(spawnPoint);
        }
        if (weight[(int)rank] * 20 < randValue)
        {
            return;
        }

        randValue = rand.Next(0, 3);
        ItemType type = (ItemType)randValue;
        Item.UsingItem target;
        int findIdx = itemPool.FindIndex((el) => el.type == type);
        if (findIdx < 0)
        {
            target = Instantiate(ItemPrefab, spawnPoint, Quaternion.identity, objectPool[(int)PoolType.Item]);
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

    public void DropStoryItem(Vector3 spawnPoint)
    {
        Item.StoryItem target;
        target = Instantiate(StoryItemPrefab, spawnPoint, Quaternion.identity, objectPool[(int)PoolType.Item]);
        Instantiate(ResourceManager.Instance.itemInfos[(int)ItemType.Story].thisParticle, target.transform);
    }

#if UNITY_EDITOR
    public void ItemTest()//개발자 모드용
    {
        Item.UsingItem target;
        target = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity, objectPool[(int)PoolType.Item]);
        Instantiate(ResourceManager.Instance.itemInfos[(int)2].thisParticle, target.transform);
        target.type = ItemType.Boost;
    }
#endif
}