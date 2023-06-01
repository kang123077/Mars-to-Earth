using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ReportCheckPPUI : MonoBehaviour
{
    public List<PlayerSaveInfo> playerSaveInfoList;
    public List<ReportContentUIController> reportContentList;
    public GameObject reportGameContent;
    public Transform cloneParentsTF;
    public Scrollbar reportScroll;
    private int saveCount;
    private float scrollInput;

    private void Awake()
    {
        playerSaveInfoList = LoadPlayerSaveInfo();
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
            if (PlayerPrefs.HasKey("save" + saveCount.ToString()))
            {
                string jsonData = PlayerPrefs.GetString("save" + saveCount.ToString());
                Debug.Log(jsonData);
                PlayerSaveInfo saveInfo = JsonUtility.FromJson<PlayerSaveInfo>(jsonData);
                saveInfoList.Add(saveInfo);
            }
        }
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
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f)
        {
            scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
            reportScroll.value += scrollInput;
        }
    }
}
