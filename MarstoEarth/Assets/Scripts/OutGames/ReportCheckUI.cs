using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ReportCheckUI : MonoBehaviour
{
    public Scrollbar reportScroll;
    int recordClearRoom;
    public TMPro.TextMeshProUGUI reportclearRoom;
    public TMPro.TextMeshProUGUI reportclearTime;

    void Start()
    {
        TextAsset txtAsset = Resources.Load<TextAsset>("Jsons/TestJson2");
        JSONNode root = JSON.Parse(txtAsset.text);
        JSONNode data1 = root["D1"];
        int score = data1["Score"].AsInt;
        string name = data1["userID"].Value;
        reportclearRoom.text = score.ToString();
        reportclearTime.text = name;
    }

    public void ReportGame()
    {
        gameObject.SetActive(true);
        recordClearRoom = InGameManager.clearedRooms + InGameManager.clearedBossRoom;
        // reportText.text = "Clear Room = " + recordClearRoom + "\n" + "Clear Time = " + Time.realtimeSinceStartup;
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
