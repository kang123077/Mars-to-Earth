using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    public Slider maserVolume;
    public Slider BGMVolume;
    public Slider effectVolume;
    public TMPro.TMP_Dropdown resolutionCon;

    void Start()
    {
        resolutionCon = GetComponentInChildren<TMPro.TMP_Dropdown>(); // try get
        maserVolume.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });
        BGMVolume.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        effectVolume.onValueChanged.AddListener(delegate { OnEffectVolumeChanged(); });
        resolutionCon.onValueChanged.AddListener(delegate { OnResolutionChanged(); });
        gameObject.SetActive(false); // UI 비활성화
    }

    public void OffSettingUI()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void OnResolutionChanged()
    {
        int value = resolutionCon.value;

        switch (value)
        {
            case 0: // 1920 x 1080
                Screen.SetResolution(1920, 1080, true);
                Debug.Log(Screen.currentResolution);
                break;
            case 1: // 1024 x 768
                Screen.SetResolution(1024, 768, false);
                Debug.Log(Screen.currentResolution);
                break;
            case 2: // 800 x 600
                Screen.SetResolution(800, 600, false);
                Debug.Log(Screen.currentResolution);
                break;
            default:
                break;
        }
    }

    public void OnMasterVolumeChanged()
    {
        AudioManager.Instance.SetMasterVolume(maserVolume.value);
    }

    public void OnBGMVolumeChanged()
    {
        AudioManager.Instance.SetBGMVolume(BGMVolume.value);
    }

    public void OnEffectVolumeChanged()
    {
        AudioManager.Instance.SetEffectVolume(effectVolume.value);
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
