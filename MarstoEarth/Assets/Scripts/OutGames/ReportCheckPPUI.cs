using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ReportCheckPPUI : MonoBehaviour
{
    public List<ReportContentUIController> reportContentList;
    public GameObject reportGameContent;
    public Transform cloneParentsTF;
    public Scrollbar reportScroll;
    private List<PlayerSaveInfo> playerSaveInfoList;
    private int saveCount;
    private float scrollInput;

    private void Awake()
    {
        playerSaveInfoList = LoadPlayerSaveInfo();
    }

    private void Start()
    {
        InitContent();
    }

    private List<PlayerSaveInfo> LoadPlayerSaveInfo()
    {
        List<PlayerSaveInfo> saveInfoList = new List<PlayerSaveInfo>();

        if (PlayerPrefs.HasKey("saveCount"))
        {
            // saveCount 확인, 있으면 가져옴
            saveCount = PlayerPrefs.GetInt("saveCount");
        }
        else
        {
            // 없으므로 그냥 return
            saveCount = 0;
            return saveInfoList;
        }

        for (int i = 0; i < saveCount; i++)
        {
            // saveCount 횟수만큼 가져와서 직렬화한 후 넣어준다
            if (PlayerPrefs.HasKey("save" + i.ToString()))
            {
                string jsonData = PlayerPrefs.GetString("save" + i.ToString());
                PlayerSaveInfo saveInfo = JsonUtility.FromJson<PlayerSaveInfo>(jsonData);
                saveInfoList.Add(saveInfo);
            }
        }
        return saveInfoList;
    }

    private void InitContent()
    {
        foreach (PlayerSaveInfo playerSaveInfo in playerSaveInfoList)
        {
            ReportContentUIController reportContent =
                Instantiate(reportGameContent, cloneParentsTF).GetComponent<ReportContentUIController>();
            reportContent.InitContent(playerSaveInfo);
            reportContentList.Add(reportContent);
        }
    }

    public void DataChoiceDelete()
    {
        List<PlayerSaveInfo> infoToRemove = new List<PlayerSaveInfo>();
        List<ReportContentUIController> contentToRemove = new List<ReportContentUIController>();
        for (int i = 0; i < reportContentList.Count; i++)
        {
            if (reportContentList[i].contentToggle.isOn)
            {
                infoToRemove.Add(playerSaveInfoList[i]);
                contentToRemove.Add(reportContentList[i]);
            }
        }
        // PlayerSaveInfo 체크
        foreach (PlayerSaveInfo removeInfo in infoToRemove)
        {
            playerSaveInfoList.Remove(removeInfo);
        }
        // ReportContentUIController 체크
        foreach (ReportContentUIController removeContent in contentToRemove)
        {
            Destroy(removeContent.gameObject);
            reportContentList.Remove(removeContent);
        }
        // 오염을 막기 위해 먼저 한번 다 지워 줌
        PlayerPrefs.DeleteAll();
        saveCount = 0;
        // 남은 playerSaveInfo 다시 저장해줌
        foreach (PlayerSaveInfo playerSaveInfo in playerSaveInfoList)
        {
            SavePPContent(playerSaveInfo);
        }
        PlayerPrefs.Save();
    }

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

    public void DataAllDelete()
    {
        foreach (ReportContentUIController reportContent in reportContentList)
        {
            Destroy(reportContent.gameObject);
        }
        reportContentList.Clear();
        PlayerPrefs.DeleteAll();
    }

    public void ReportGame()
    {
        // Title Button UI에서 사용
        OutGameAudio.Instance.PlayEffect(1);
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f)
        {
            scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
            reportScroll.value += scrollInput;
        }
    }
}
