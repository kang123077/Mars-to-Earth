using UnityEngine;
using UnityEngine.UI;

public class ReportCheckUI : MonoBehaviour
{
    public Scrollbar reportScroll;
    int recordClearRoom;
    // public TMPro.TextMeshProUGUI reportText;

    void Start()
    {
        
    }

    public void ReportGame()
    {
        gameObject.SetActive(true);
        recordClearRoom = InGameManager.clearedRooms + InGameManager.clearedBossRoom;
        // reportText.text = "Clear Room = " + recordClearRoom + "\n" + "Clear Time = " + Time.realtimeSinceStartup;
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
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
