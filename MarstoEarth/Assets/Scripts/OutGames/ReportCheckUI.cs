using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;


[Serializable]
public class ClearReport
{
    int clearRooms;
    float clearTimes;
    public int ClearRoom
    {
        get { return clearRooms; }
        set { clearRooms = value; }
    }
    public float ClearTime
    {
        get { return clearTimes; }
        set { clearTimes = value; }
    }
    public ClearReport()
    {
    }
    public ClearReport(int _clearRooms, float _clearTimes)
    {
        clearRooms = _clearRooms;
        clearTimes = _clearTimes;
    }
}
[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> _t;
    public List<T> ToList() { return _t; }
    public Serialization(List<T> _tmp)
    {
        _t = _tmp;
    }
}
public class ReportCheckUI : MonoBehaviour
{
    public Scrollbar reportScroll;
    public TMPro.TextMeshProUGUI reportclearRoom;
    public TMPro.TextMeshProUGUI reportclearTime;
    public GameObject reportGameClone;
    public Transform cloneParents;
    List<ClearReport> clearReportList;

    private void Awake()
    {
        clearReportList = new List<ClearReport>();
    }
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            ClearReport clearReport = new ClearReport();
            clearReport.ClearRoom = i; //InGameManager.clearedBossRoom + InGameManager.clearedBossRoom;
            clearReport.ClearTime = i; //Time.deltaTime;
            clearReportList.Add(clearReport);
        }
        // List -> Json
        string jsonDataList = JsonUtility.ToJson(new Serialization<ClearReport>(clearReportList));
        using (StreamWriter sr = new StreamWriter(Application.dataPath + "/" + "ClearDatas.json"))
        {
            sr.WriteLine(jsonDataList);
            sr.Close();
        }
        // Json -> List
        List<ClearReport> clearReportToList = JsonUtility.FromJson<Serialization<ClearReport>>(jsonDataList).ToList();
        foreach(ClearReport one in clearReportToList)
        {
            reportclearRoom.text = one.ClearRoom.ToString();
            reportclearTime.text = one.ClearTime.ToString();
        }
        for (int i = 0; i < clearReportList.Count; i++)
        {
            GameObject newObject = Instantiate(reportGameClone, cloneParents);
            // 추가적인 설정이 필요한 경우 newObject를 조작합니다.
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
        if (reportScroll.value <= 0)
        {
            reportScroll.value = 0f;
        }
        else if (reportScroll.value >= 1)
        {
            reportScroll.value = 1f;
        }
    }
}
