using UnityEngine;

public class MobileSettingUI : UI
{
    public UnityEngine.UI.Slider maserVolume;
    public UnityEngine.UI.Slider BGMVolume;
    public UnityEngine.UI.Slider effectVolume;

    void Start()
    {
        maserVolume.onValueChanged.AddListener(delegate { MOnMasterVolumeChanged(); });
        BGMVolume.onValueChanged.AddListener(delegate { MOnBGMVolumeChanged(); });
        effectVolume.onValueChanged.AddListener(delegate { MOnEffectVolumeChanged(); });
        BGMVolume.value = AudioManager.bgmVolume;
        effectVolume.value = AudioManager.effectVolume;
        gameObject.SetActive(false); // UI 비활성화
    }

    public void MOffSettingUI()
    {
        if (UIManager.Instance.UIs[(int)UIType.Card].gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            if (gameObject.activeSelf == false)
            {
                MapInfo.pauseRequest--;
            }
        }
    }

    public void MOnMasterVolumeChanged()
    {
        AudioManager.Instance.SetMasterVolume(maserVolume.value);
    }

    public void MOnBGMVolumeChanged()
    {
        AudioManager.Instance.SetBGMVolume(BGMVolume.value);
    }

    public void MOnEffectVolumeChanged()
    {
        AudioManager.Instance.SetEffectVolume(effectVolume.value);
    }

    public void MGameGoTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OutGameScene");
        MapInfo.pauseRequest--;
    }

    public void MGameExit()
    {
        Application.Quit();
    }
}
