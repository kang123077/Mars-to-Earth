using TMPro;
using UnityEngine;

/// <summary>
/// 여기선 보여주기만 하고, 실제 값은 MapManager에서 변경
/// </summary>
public class GameInfoUIController : MonoBehaviour
{
    public TMP_Text stageUI;
    public TMP_Text timeUI;
    public TMP_Text levelUI;

    private void Start()
    {
        StageUIUpdate();
    }

    private void Update()
    {
        // update가 MapManager 합쳐서 두 개 도는데, 고치면 좋긴 하지만...
        TimeUIUpdate();
        LevelUIUpdate();
    }

    public void StageUIUpdate()
    {
        string stageText = "Stage " + MapInfo.cur_Stage.ToString();
        stageUI.text = stageText;
    }

    public void TimeUIUpdate()
    {
        int minutes = Mathf.FloorToInt(MapInfo.cur_Time / 60);
        int seconds = Mathf.FloorToInt(MapInfo.cur_Time % 60);

        string formattedTime = minutes.ToString("00") + " : " + seconds.ToString("00");
        timeUI.text = formattedTime;
    }

    public void LevelUIUpdate()
    {
        levelUI.text = Mathf.Floor(MapInfo.difficulty).ToString("F0");
    }
}
