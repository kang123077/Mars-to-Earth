using TMPro;
using UnityEngine;
using static GameoverUIController;

public class ReportContentUIController : MonoBehaviour
{
    public TMP_Text date;
    public TMP_Text time;
    public TMP_Text clearedStage;
    public TMP_Text clearedRooms;
    public TMP_Text playedTime;
    public TMP_Text hpCoreAmount;
    public TMP_Text dmgCoreAmount;
    public TMP_Text speedCoreAmount;
    public PlayerStatUIController playerStatUI;

    public void InitContent(PlayerInfo playerInfo)
    {
        date.text = playerInfo.date;
        time.text = playerInfo.time;
        clearedStage.text = playerInfo.clearedStage.ToString();
        clearedRooms.text = playerInfo.clearedRooms.ToString();
        playedTime.text = playerInfo.playedTime;
        hpCoreAmount.text = playerInfo.hpCoreAmount.ToString();
        dmgCoreAmount.text = playerInfo.dmgCoreAmount.ToString();
        speedCoreAmount.text = playerInfo.speedCoreAmount.ToString();
    }

    public void InitPlayerStat()
    {
        playerStatUI.InitStaticStat();
    }
}
