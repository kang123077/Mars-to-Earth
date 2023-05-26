using System.Collections.Generic;
using UnityEngine;

public class OutGameSettingUI : MonoBehaviour
{
    public UnityEngine.UI.Slider maserVolume;
    public int resolutionNum;
    public TMPro.TMP_Dropdown resolutionCon;
    FullScreenMode screenMode;
    public UnityEngine.UI.Toggle fullScreen;
    List<Resolution> resolutions;
    

    private void Awake()
    {
        resolutions = new List<Resolution>();
    }

    void Start()
    {
        resolutionCon = GetComponentInChildren<TMPro.TMP_Dropdown>();
        maserVolume.onValueChanged.AddListener(delegate { SetMasterVolume(); } );
        ResolInit();
    }

    void ResolInit()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            float aspectRatio = (float)Screen.resolutions[i].width / Screen.resolutions[i].height;
            if (Mathf.Approximately(aspectRatio, 16f / 9f))
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        resolutionCon.options.Clear();
        resolutionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMPro.TMP_Dropdown.OptionData option = new TMPro.TMP_Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " ";
            resolutionCon.options.Add(option);
            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionCon.value = resolutionNum;
            }
            resolutionNum++;
        }
        resolutionCon.RefreshShownValue();
        fullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void ResolNumChange(int x)
    {
        resolutionNum = x;
    }

    public void OnResolutionChanged()
    {
        Debug.Log(resolutionNum);
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetMasterVolume()
    {
        float value = maserVolume.value;
        AudioManager.masterVolume = value;
        UpdateAllVolumes();
    }
    private void UpdateAllVolumes()
    {
        AudioManager.finalBGM_Volume = AudioManager.masterVolume * AudioManager.bgmVolume;
        AudioManager.finalEffectVolume = AudioManager.masterVolume * AudioManager.effectVolume;
    }

    public void GameSettingUI()
    {
        gameObject.SetActive(true);
    }
}
