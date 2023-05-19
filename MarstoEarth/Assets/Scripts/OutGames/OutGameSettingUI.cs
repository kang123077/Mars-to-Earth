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
        resolutionCon = GetComponentInChildren<TMPro.TMP_Dropdown>(); // try get
        maserVolume.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });
        ResolInit();
    }

    void ResolInit()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width * 9 == Screen.resolutions[i].height * 16)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        resolutionCon.ClearOptions();

        resolutionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMPro.TMP_Dropdown.OptionData option = new TMPro.TMP_Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " ";
            resolutionCon.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionCon.value = resolutionNum;
                resolutionNum++;
            }
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
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OnMasterVolumeChanged()
    {
        AudioManager.Instance.SetMasterVolume(maserVolume.value);
    }

    public void GameSettingUI()
    {
        gameObject.SetActive(true);
    }


    void Update()
    {
        
    }
}
