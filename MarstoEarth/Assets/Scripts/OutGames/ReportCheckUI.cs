using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class ClearReport
{
    [SerializeField]int clearRooms;
    [SerializeField]float clearTimes;
    public int CLEARROOM
    {
        get { return clearRooms; }
        set { clearRooms = value; }
    }
    public float CLEARTIME
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
    public GameObject reportGameClone;
    public Transform cloneParents;
    List<ClearReport> clearReportList;

    private void Awake()
    {
        clearReportList = new List<ClearReport>();
    }
    void Start()
    {
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
    }

    public void DataChoiceDelete()
    {
        
    }

    public void DataAllDelete()
    {
        int childCount = cloneParents.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(cloneParents.GetChild(i).gameObject);
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
