using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    public Slider maserVolume;
    public Slider BGMVolume;
    public Slider effectVolume;
    public TMPro.TMP_Dropdown resolutionCon;
    public int resolutionNum;
    FullScreenMode screenMode;
    public Toggle fullScreen;
    List<Resolution> resolutions;

    private void Awake()
    {
        resolutions = new List<Resolution>();
    }

    void Start()
    {
        resolutionCon = GetComponentInChildren<TMPro.TMP_Dropdown>();
        maserVolume.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });
        BGMVolume.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        effectVolume.onValueChanged.AddListener(delegate { OnEffectVolumeChanged(); });
        ResolInit();
        gameObject.SetActive(false); // UI 비활성화
    }

    public void OffSettingUI()
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
                Time.timeScale = 1f;
            }
        }
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
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height, screenMode);
    }

    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
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

    public void GameGoTitle()
    {
        Debug.Log("게임을 재실행 합니다.");
        UnityEngine.SceneManagement.SceneManager.LoadScene("OutGameScene");
        Time.timeScale = 1.0f;
    }

    public void GameExit()
    {
        Debug.Log("게임을 나갑니다.");
        Application.Quit();
    }
}
