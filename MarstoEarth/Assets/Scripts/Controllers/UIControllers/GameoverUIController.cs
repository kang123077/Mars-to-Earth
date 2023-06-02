using Character;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverUIController : MonoBehaviour
{
    public ReportContentUIController reportContent;
    private int saveCount;

    private void OnEnable()
    {
        SaveReportContent();
    }

    public void GotoTitle()
    {
        // UI 버튼에서 사용
        Time.timeScale = 1f;
        InGameManager.Instance.panel.SetActive(true);
        SceneManager.LoadScene("OutGameScene");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// PP = PlayerPrefs
    /// </summary>
    public void SavePPContent(PlayerSaveInfo playerSaveInfo)
    {
        // JSON 형식으로 직렬화
        string jsonData = JsonUtility.ToJson(playerSaveInfo, true);
        // 지금까지 save들의 수 확인, 없으면 0
        if (PlayerPrefs.HasKey("saveCount"))
            saveCount = PlayerPrefs.GetInt("saveCount");
        else
            saveCount = 0;
        // 현재 Save Idx에 저장
        // ex : save0, save1, save2
        PlayerPrefs.SetString("save" + saveCount.ToString(), jsonData);
        // Save를 하나 추가
        PlayerPrefs.SetInt("saveCount", saveCount + 1);
    }

    public void SaveReportContent()
    {
        // 유저 정보를 JSON 객체로 생성
        PlayerSaveInfo playerSaveInfo = new PlayerSaveInfo();
        // 현재 시간을 가져옴
        DateTime currentDateTime = DateTime.Now;
        // 날짜와 시간을 원하는 형식으로 변환
        string formattedDate = currentDateTime.ToString("yyyy / MM / dd");
        string formattedTime = currentDateTime.ToString("HH : mm");
        playerSaveInfo.date = formattedDate;
        playerSaveInfo.time = formattedTime;
        int intClearedStage = MapInfo.cur_Stage - 1;
        playerSaveInfo.clearedStage = intClearedStage;
        playerSaveInfo.clearedRooms = InGameManager.clearedRooms;
        playerSaveInfo.playedTime = UIManager.Instance.gameInfoUIController.timeUI.text;
        playerSaveInfo.hpCoreAmount = MapInfo.hpCore;
        playerSaveInfo.dmgCoreAmount = MapInfo.dmgCore;
        playerSaveInfo.speedCoreAmount = MapInfo.speedCore;
        playerSaveInfo.core = MapInfo.core;
        playerSaveInfo.maxHp = staticStat.maxHP;
        playerSaveInfo.attack = staticStat.dmg;
        playerSaveInfo.speed = staticStat.speed;
        playerSaveInfo.defence = staticStat.def;
        playerSaveInfo.duration = staticStat.duration;
        playerSaveInfo.range = staticStat.range;
        playerSaveInfo.fileName = currentDateTime.ToString("yyyMMdd") + currentDateTime.ToString("HHmm");

        SavePPContent(playerSaveInfo);
        InitReportContent(playerSaveInfo);
    }

    public void InitReportContent(PlayerSaveInfo playerInfo)
    {
        reportContent.InitContent(playerInfo);
    }
}
