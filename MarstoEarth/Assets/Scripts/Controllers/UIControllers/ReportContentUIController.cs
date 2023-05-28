using TMPro;
using UnityEngine;

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

    public void InitContent(PlayerSaveInfo playerSaveInfo)
    {
        date.text = playerSaveInfo.date;
        time.text = playerSaveInfo.time;
        clearedStage.text = playerSaveInfo.clearedStage.ToString();
        clearedRooms.text = playerSaveInfo.clearedRooms.ToString();
        playedTime.text = playerSaveInfo.playedTime;
        hpCoreAmount.text = playerSaveInfo.hpCoreAmount.ToString();
        dmgCoreAmount.text = playerSaveInfo.dmgCoreAmount.ToString();
        speedCoreAmount.text = playerSaveInfo.speedCoreAmount.ToString();
        InitPlayerStat(playerSaveInfo);
    }

    public void InitPlayerStat(PlayerSaveInfo playerSaveInfo)
    {
        playerStatUI.InitPlayerInfoUI(playerSaveInfo);
    }
}
