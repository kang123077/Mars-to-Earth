using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public static int clearedRooms = 0; // 클리어한 룸 수를 저장하는 변수
    public static int clearedBossRoom = 0;
    public GameObject cardUICon;
    public GameObject reinforceCardUICon;
    public CardInfo cardInfo;
    public ReinforceInfo reinforceInfo;
    public List<Skill.Skill> inGameSkill;
    public GameObject panel; // 패널을 활성화 및 비활성화를 통해 씬 전체의 화면을 가리게 만들었음 추후 더 좋은 구조 찾아서 변경할 것

    protected override void Awake()
    {
        base.Awake();
        // 스킬들을 리스트로 만듦
        inGameSkill = ResourceManager.Instance.skills.ToList();
        panel.SetActive(false);
    }

    private void Start()
    {
        // CardUI 부모 객체를 끔
        cardUICon.SetActive(false);
        reinforceCardUICon.SetActive(false);
        InitInGame();
    }

    private void TriggerEvent()
    {
        MapInfo.pauseRequest++;
        AudioManager.Instance.PlayEffect(1);
        AudioManager.Instance.PauseSource();
        UIManager.Instance.aimImage.gameObject.SetActive(false);
        cardUICon.SetActive(true);
        cardInfo.CardInit();
    }

    // 맵 클리어 시 발동 함수
    public void OnRoomCleared()
    {
        clearedRooms++;
        if (clearedRooms % 1 == 0 && !CombatUI.fullCheck == true)
        {
            TriggerEvent();
        }
        else if (clearedRooms % 1 == 0 && CombatUI.fullCheck == true)
        {
            ReinforceEvent();
        }
    }

    private void ReinforceEvent()
    {
        reinforceCardUICon.SetActive(true);
        reinforceInfo.ReinforceAdd();
    }

    public void OnBossCleared()
    {
        clearedBossRoom++;
        Vector3 point = SpawnManager.Instance.playerTransform.position;
        point.y = 0.8f;

        for (ushort i = 0; i < 8; i++)
        {
            SpawnManager.Instance.DropItem(point + Vector3.right *
                SpawnManager.rand.Next(-8, 8) + Vector3.forward * SpawnManager.rand.Next(-8, 8), EnemyPool.Boss);
        }
        Character.staticStat.ResetValues();
    }

    public void InitInGame()
    {
        MapManager.Instance.GenerateMapCall();
        foreach (PathController path in MapManager.Instance.paths)
        {
            path.InitPath();
        }
        SpawnManager.Instance.InitSpawn();
    }
}
