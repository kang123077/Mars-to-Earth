using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public static int clearedRooms = 0; // 클리어한 룸 수를 저장하는 변수
    public static int clearedBossRoom = 0;
    public GameObject cardUICon;
    public CardInfo cardInfo;
    public List<Skill.Skill> inGameSkill;

    protected override void Awake()
    {
        base.Awake();
        // 스킬들을 리스트로 만듦
        inGameSkill = ResourceManager.Instance.skills.ToList();
    }

    private void Start()
    {
        // CardUI 부모 객체를 끔
        cardUICon.SetActive(false);
        InitInGame();
    }

    private void TriggerEvent()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.PlayEffect(1);
        AudioManager.Instance.PauseSource();
        UIManager.Instance.aimImage.gameObject.SetActive(false);
        cardUICon.SetActive(true);
        cardInfo.CardInit();
    }

    ///*
    // * 맵을 클리어할 때 이벤트를 발생시키고
    // * 맵의 특정한 구역에 적용하면 구역 통과시
    // * 맵 클리어할 때 발동되는 코드
    // */
    public void OnRoomCleared()
    {
        clearedRooms++;
        //if (clearedRooms % 2 == 0) // 2 개의 방을 클리어했을 때
        //{
        //    TriggerEvent();
        //}
        if(clearedBossRoom % 1 == 0)
        {
            TriggerEvent();
        }
    }

    public void OnBossCleared()
    {
        clearedBossRoom++;
        Debug.Log("보스 클리어 보상 이벤트 함수");
    }

    public void InitInGame()
    {
        MapManager.Instance.GenerateMapCall();
        foreach(PathController path in MapManager.Instance.paths)
        {
            path.InitPath();
        }
        SpawnManager.Instance.InitSpawn();
    }
}
