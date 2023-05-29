using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

public class ReportCheckUI : MonoBehaviour
{
    private string saveFolderPath;
    public List<PlayerSaveInfo> playerSaveInfoList;
    public List<ReportContentUIController> reportContentList;
    public GameObject reportGameContent;
    public Transform cloneParentsTF;
    public Scrollbar reportScroll;

    private void Awake()
    {
        saveFolderPath = System.IO.Path.Combine(Application.dataPath, "Record");
        playerSaveInfoList = LoadPlayerSaveInfo();
    }

    private List<PlayerSaveInfo> LoadPlayerSaveInfo()
    {
        List<PlayerSaveInfo> saveInfoList = new List<PlayerSaveInfo>();

        // 폴더 내의 모든 JSON 파일에 대해 반복
        string[] filePaths = Directory.GetFiles(saveFolderPath, "*.json");
        foreach (string filePath in filePaths)
        {
            string json = File.ReadAllText(filePath);
            PlayerSaveInfo saveInfo = JsonUtility.FromJson<PlayerSaveInfo>(json);
            saveInfoList.Add(saveInfo);
        }
        return saveInfoList;
    }

    void Start()
    {
        foreach(PlayerSaveInfo playerSaveInfo in playerSaveInfoList)
        {
            ReportContentUIController reportContent =
                Instantiate(reportGameContent, cloneParentsTF).GetComponent<ReportContentUIController>();
            reportContent.InitContent(playerSaveInfo);
            reportContentList.Add(reportContent);
        }
        /*
        TMPro.TextMeshProUGUI cloneText = reportGameClone.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        for (int i = 0; i < 5; i++)
        {
            ClearReport clearReport = new ClearReport();
            clearReport.CLEARROOM = 100 + i; //InGameManager.clearedBossRoom + InGameManager.clearedBossRoom;
            clearReport.CLEARTIME = i / 10f;
            clearReportList.Add(clearReport);
        }
        // List -> Json 데이터로 저장
        string jsonDataList = JsonUtility.ToJson(new Serialization<ClearReport>(clearReportList));
        using (StreamWriter sr = new StreamWriter(Application.dataPath + "/" + "ClearData.json"))
        {
            sr.WriteLine(jsonDataList);
            sr.Close();
        }
        // Json -> List 데이터로 저장
        List<ClearReport> clearReportToList = JsonUtility.FromJson<Serialization<ClearReport>>(jsonDataList).ToList();
        foreach (ClearReport one in clearReportToList)
        {
            // cloneText.text = "ClearRooms = " + one.CLEARROOM.ToString() + "\n\n\n" + "ClearTime " + one.CLEARTIME.ToString();
            Instantiate(reportGameClone, cloneParents);
        }
        */
    }

    public void DataChoiceDelete()
    {
        
    }

    public void DataAllDelete()
    {
    }

    public void ReportGame()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        float scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
        reportScroll.value += scrollInput;
    }
}
