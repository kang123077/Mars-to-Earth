using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public int clearedRooms = 0; // 클리어한 룸 수를 저장하는 변수
    public GameObject cardUICon;
    public CardInfo cardInfo;
    public List<Skill.Skill> inGameSkill;

    //public GameObject onTargetUICon;
    protected override void Awake()
    {
        base.Awake();

        inGameSkill = ResourceManager.Instance.skills.ToList();
    }

    private void Start()
    {
        cardUICon.SetActive(false);
    }

    private void TriggerEvent()
    {
        clearedRooms = 0;
        //cardUICon.transform.SetAsLastSibling();
        Time.timeScale = 0f;
        AudioManager.Instance.PlayEffect(1);
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
        if (clearedRooms >= 1) // 2 개의 방을 클리어했을 때
        {
            TriggerEvent();
        }
    }

    //public void OnTartgetUIOn()
    //{
    //    if(CinemachineManager.Instance.bossCam.LookAt == true)
    //    {
    //        onTargetUICon.SetActive(true);
    //        onTargetUICon.transform.position = CinemachineManager.Instance.bossCam.LookAt.transform.position;
    //    }
    //}
}
