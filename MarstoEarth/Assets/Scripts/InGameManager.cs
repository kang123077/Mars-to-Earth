using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public GameObject cardUI; // 인스펙터 창에서 직접 할당할 수 있도록 변경
    private int clearedMonsters = 0; // 클리어한 몬스터 수를 저장하는 변수
    GameObject cardUICon;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        cardUICon = Instantiate(cardUI, UIManager.Instance.transform);
        cardUICon.SetActive(false);
    }
    /*
     * 몬스터를 처치할 때 이벤트를 발생시키고
     * 일정 수 이상의 몬스터를 처치하면 발동하는 코드
     */
    public void OnMonsterCleared()
    {
        clearedMonsters++;
        if (clearedMonsters >= 2) // 두 마리 이상의 몬스터를 처치했을 때
        {
            TriggerEvent();
        }
    }

    private void TriggerEvent()
    {
        Time.timeScale = 0f;
        cardUICon.SetActive(true);
    }
    //public GameObject[] cardUIs; // 인스펙터 창에서 직접 할당할 수 있도록 변경
    //private int clearedRooms = 0; // 클리어한 방의 개수를 저장하는 변수

    ///*
    // * 맵을 클리어할 때 이벤트를 발생시키고
    // * 맵의 특정한 구역에 적용하면 구역 통과시
    // * 맵 클리어할 때 발동되는 코드
    // */
    //public void OnRoomCleared()
    //{
    //    clearedRooms++;
    //    if (clearedRooms >= 2) // 두 개의 방을 클리어했을 때
    //    {
    //        TriggerEvent();
    //    }
    //}

    // 카드 정보 입력 시스템
}
