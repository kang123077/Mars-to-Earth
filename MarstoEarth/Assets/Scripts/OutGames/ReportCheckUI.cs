using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using System.Linq;

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
        // List로 Return
        return saveInfoList;
    }

    void Start()
    {
        foreach (PlayerSaveInfo playerSaveInfo in playerSaveInfoList)
        {
            ReportContentUIController reportContent =
                Instantiate(reportGameContent, cloneParentsTF).GetComponent<ReportContentUIController>();
            reportContent.InitContent(playerSaveInfo);
            reportContentList.Add(reportContent);
        }
        playerSaveInfoList.Clear();
    }

    public void DataChoiceDelete()
    {
        List<ReportContentUIController> contentToRemove = new List<ReportContentUIController>();
        foreach (ReportContentUIController reportContent in reportContentList)
        {
            if (reportContent.contentToggle.isOn == true)
            {
                contentToRemove.Add(reportContent);
            }
        }
        foreach (ReportContentUIController removeContent in contentToRemove)
        {
            reportContentList.Remove(removeContent);
            Destroy(removeContent.gameObject);

            string filePath = System.IO.Path.Combine(saveFolderPath, $"{removeContent.fileName}.json");
            string metaPath = System.IO.Path.Combine(saveFolderPath, $"{removeContent.fileName}.json.meta");

            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                Debug.Log("File Doesn't exist!");

            if (File.Exists(metaPath))
                File.Delete(metaPath);
            else
                Debug.Log("MetaFile Doesn't exist!");
        }
    }

    public void DataAllDelete()
    {
        foreach (ReportContentUIController reportContent in reportContentList)
        {
            Destroy(reportContent.gameObject);
        }
        reportContentList.Clear();
        string[] filePaths = Directory.GetFiles(saveFolderPath, "*.json");
        string[] metaPaths = Directory.GetFiles(saveFolderPath, "*.meta");
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }
        foreach (string metaPath in metaPaths)
        {
            File.Delete(metaPath);
        }
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
