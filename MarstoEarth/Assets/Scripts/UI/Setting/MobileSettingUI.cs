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
        AudioManager.masterVolume = value;
        AudioManager.bgmAudioSource.volume = value * BGMVolume.value;
        AudioManager.finalEffectVolume = value;
    }

    public void MOnBGMVolumeChanged()
    {
        float value = BGMVolume.value;
        AudioManager.bgmAudioSource.volume = value * maserVolume.value;
        AudioManager.bgmVolume = value;
    }

    public void MOnEffectVolumeChanged()
    {
        float value = effectVolume.value;
        AudioManager.effectAudioSource.volume = value;
        AudioManager.finalEffectVolume = value;
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
