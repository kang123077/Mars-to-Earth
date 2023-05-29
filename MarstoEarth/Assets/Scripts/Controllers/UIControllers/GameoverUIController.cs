using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Character;

public class GameoverUIController : MonoBehaviour
{
    public ReportContentUIController reportContent;

    private void OnEnable()
    {
        SaveReportContent();
    }

    public void GotoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OutGameScene");
        gameObject.SetActive(false);
    }

    public void InitReportContent(PlayerSaveInfo playerInfo)
    {
        reportContent.InitContent(playerInfo);
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

        // JSON 형식으로 직렬화
        string jsonData = JsonUtility.ToJson(playerSaveInfo, true);
        // 파일 이름 설정 (확장자 .json 추가)
        string fileName = $"{currentDateTime.ToString("yyyyMMdd")}{currentDateTime.ToString("HHmm")}.json";
        // 파일 경로 설정 (persistentDataPath 사용)
        string filePath = System.IO.Path.Combine(Application.dataPath, "Record", fileName);
        // List -> Json 데이터로 저장
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(jsonData);
        }

        InitReportContent(playerSaveInfo);
    }
}
