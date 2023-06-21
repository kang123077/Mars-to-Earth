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
        gameObject.SetActive(false);
        MapInfo.pauseRequest--;
    }

    public void MOnMasterVolumeChanged()
    {
        float value = maserVolume.value;
        AudioManager.bgmAudioSource.volume = value;
        AudioManager.masterVolume = value;
    }

    public void MOnBGMVolumeChanged()
    {
        float value = BGMVolume.value;
        AudioManager.bgmAudioSource.volume = value;
        AudioManager.bgmVolume = value;
    }

    public void MOnEffectVolumeChanged()
    {
        float value = BGMVolume.value;
        AudioManager.effectAudioSource.volume = value;
        AudioManager.effectVolume = value;
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
