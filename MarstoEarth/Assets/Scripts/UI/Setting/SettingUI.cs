using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    public Slider maserVolume;
    public Slider BGMVolume;
    public Slider effectVolume;
    private Dropdown resolutionCon;

    void Start()
    {
        resolutionCon = GetComponentInChildren<Dropdown>();
        gameObject.SetActive(false); // UI 비활성화
    }

    public void OffSettingUI()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void ResolutionChange()
    {

    }

    public void GameReStart()
    {
        Debug.Log("게임을 재실행 합니다.");
    }

    public void GameExit()
    {
        Debug.Log("게임을 나갑니다.");
    }
}
