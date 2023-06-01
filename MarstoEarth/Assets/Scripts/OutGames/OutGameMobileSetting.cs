using UnityEngine;

public class OutGameMobileSetting : MonoBehaviour
{
    public UnityEngine.UI.Slider ogsbgmVolume;
    public UnityEngine.UI.Slider ogseffectVolume;
    
    void Start()
    {
        ogsbgmVolume.onValueChanged.AddListener(delegate { SetBGMVolume(); });
        ogseffectVolume.onValueChanged.AddListener(delegate { SetEffectVolume(); });
    }

    public void SetBGMVolume()
    {
        float value = ogsbgmVolume.value;
        AudioManager.bgmVolume = value;
        UpdateAllVolumes();
    }

    public void SetEffectVolume()
    {
        float value = ogseffectVolume.value;
        AudioManager.effectVolume = value;
        UpdateAllVolumes();
    }

    private void UpdateAllVolumes()
    {
        AudioManager.finalBGM_Volume = AudioManager.masterVolume * AudioManager.bgmVolume;
        AudioManager.finalEffectVolume = AudioManager.masterVolume * AudioManager.effectVolume;
    }
}
