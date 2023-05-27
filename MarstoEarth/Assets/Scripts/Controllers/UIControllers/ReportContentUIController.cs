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
    public PlayerStatUIController playerStatUI;

    public void InitContent()
    {
        InitDateTime();
        string stageString = UIManager.Instance.gameInfoUIController.stageUI.text;
        string cleanedString = stageString.Replace("Stage", "").Trim();
        clearedStage.text = cleanedString;
        clearedRooms.text = InGameManager.clearedRooms.ToString();
        playedTime.text = UIManager.Instance.gameInfoUIController.timeUI.text;
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
