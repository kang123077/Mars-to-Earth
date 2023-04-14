using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    private int clearedRooms = 0; // 클리어한 룸 수를 저장하는 변수
    //public GameObject cardUICon;
    //public GameObject skillUICon;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //cardUICon.SetActive(false);
    }
    /*
     * 몬스터를 처치할 때 이벤트를 발생시키고
     * 일정 수 이상의 몬스터를 처치하면 발동하는 코드
     */

    private void TriggerEvent()
    {
        //skillUICon.SetActive(false);
        //cardUICon.transform.SetAsLastSibling();
        Time.timeScale = 0f;
        //cardUICon.SetActive(true);
    }
    ///*
    // * 맵을 클리어할 때 이벤트를 발생시키고
    // * 맵의 특정한 구역에 적용하면 구역 통과시
    // * 맵 클리어할 때 발동되는 코드
    // */
    public void OnRoomCleared()
    {
        Debug.Log("온룸클리어 함수를 호출했습니다.");
        clearedRooms++;
        if (clearedRooms >= 4) // 4 개의 방을 클리어했을 때
        {
            TriggerEvent();
        }
    }
}
