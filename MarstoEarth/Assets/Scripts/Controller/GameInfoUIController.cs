using TMPro;
using UnityEngine;

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
        levelUI.text = MapInfo.difficulty.ToString("00");
    }
}
