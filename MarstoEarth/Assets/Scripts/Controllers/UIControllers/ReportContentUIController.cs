using System;
using TMPro;
using UnityEngine;

public class ReportContentUIController : MonoBehaviour
{
    public TMP_Text date;
    public TMP_Text time;
    public TMP_Text clearedStage;
    public TMP_Text clearedRooms;
    public TMP_Text playedTime;
    public TMP_Text hpCoreAmount;
    public TMP_Text dmgCoreAmount;
    public TMP_Text speedCoreAmount;
    public PlayerStatUIController playerStatUI;

    public void InitContent()
    {
        InitDateTime();
        int intClearedStage = MapInfo.cur_Stage - 1;
        clearedStage.text = intClearedStage.ToString();
        clearedRooms.text = InGameManager.clearedRooms.ToString();
        playedTime.text = UIManager.Instance.gameInfoUIController.timeUI.text;
        hpCoreAmount.text = MapInfo.hpCore.ToString();
        dmgCoreAmount.text = MapInfo.dmgCore.ToString();
        speedCoreAmount.text = MapInfo.speedCore.ToString();
        InitPlayerStat();
    }

    private void InitDateTime()
    {
        // 현재 시간을 가져옴
        DateTime currentDateTime = DateTime.Now;
        // 날짜와 시간을 원하는 형식으로 변환
        string formattedDateTime = currentDateTime.ToString("yyyy / MM / dd");
        string formattedTime = currentDateTime.ToString("HH : mm");
        // UI에 텍스트로 표시
        date.text = formattedDateTime;
        time.text = formattedTime;
    }

    public void InitPlayerStat()
    {
        playerStatUI.InitStaticStat();
    }
}
