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

    public void InitReportContent(PlayerInfo playerInfo)
    {
        reportContent.InitContent(playerInfo);
    }

    public void SaveReportContent()
    {
        // 유저 정보를 JSON 객체로 생성
        PlayerInfo playerInfo = new PlayerInfo();
        // 현재 시간을 가져옴
        DateTime currentDateTime = DateTime.Now;
        // 날짜와 시간을 원하는 형식으로 변환
        string formattedDate = currentDateTime.ToString("yyyy / MM / dd");
        string formattedTime = currentDateTime.ToString("HH : mm");
        playerInfo.date = formattedDate;
        playerInfo.time = formattedTime;
        int intClearedStage = MapInfo.cur_Stage - 1;
        playerInfo.clearedStage = intClearedStage;
        playerInfo.clearedRooms = InGameManager.clearedRooms;
        playerInfo.playedTime = UIManager.Instance.gameInfoUIController.timeUI.text;
        playerInfo.hpCoreAmount = MapInfo.hpCore;
        playerInfo.dmgCoreAmount = MapInfo.dmgCore;
        playerInfo.speedCoreAmount = MapInfo.speedCore;
        playerInfo.core = MapInfo.core;
        playerInfo.maxHp = staticStat.maxHP;
        playerInfo.attack = staticStat.dmg;
        playerInfo.speed = staticStat.speed;
        playerInfo.defence = staticStat.def;
        playerInfo.duration = staticStat.duration;
        playerInfo.range = staticStat.range;

        // JSON 형식으로 직렬화
        string jsonData = JsonUtility.ToJson(playerInfo);
        // 파일 이름 설정 (확장자 .json 추가)
        string fileName = $"{currentDateTime.ToString("yyyyMMdd")}{currentDateTime.ToString("HHmm")}.json";
        // 파일 경로 설정 (persistentDataPath 사용)
        string filePath = System.IO.Path.Combine(Application.dataPath, "Record", fileName);
        // List -> Json 데이터로 저장
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(jsonData);
        }
        InitReportContent(playerInfo);
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public string date;
        public string time;
        public int clearedStage;
        public int clearedRooms;
        public string playedTime;
        public int hpCoreAmount;
        public int dmgCoreAmount;
        public int speedCoreAmount;
        public int core;
        public float maxHp;
        public float attack;
        public float speed;
        public float defence;
        public float duration;
        public float range;
    }
}
