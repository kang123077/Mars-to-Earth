using UnityEngine;

public class OutGameUIManager : Singleton<OutGameUIManager>
{
    public int recordClearRoom;
    public GameObject[] UIcontroll;
    public AudioClip[] _BGM_AudioClips;
    public AudioClip[] _UI_EffectAudioClips;
    public GameObject[] PCMO;
    public UnityEngine.UI.Button settingButton;

    protected override void Awake()
    {
        AudioManager.BGM_AudioClips = _BGM_AudioClips;
        AudioManager.UI_EffectAudioClips = _UI_EffectAudioClips;
        for (int i = 0; i < UIcontroll.Length; i++)
        {
            UIcontroll[i].gameObject.SetActive(false);
        }
#if UNITY_ANDROID || UNITY_IOS
        PCMO[0].SetActive(false);
        PCMO[1].SetActive(true);
#else
        PCMO[0].SetActive(true);
        PCMO[1].SetActive(false);
#endif
        SetResolution();
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    private void Start()
    {
        settingButton.onClick.AddListener(OutGameSettingOn);
    }

    public void OutGameSettingOn()
    {
        if(PCMO[0] == true)
        {
            PCMO[0].transform.GetChild(0).gameObject.SetActive(true);
        }
        if(PCMO[1] == true)
        {
            PCMO[1].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ExitUI(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void GameCloser()
    {
        Application.Quit();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (var ui in UIcontroll)
            {
                if (ui.activeSelf)
                {
                    // 활성화된 UI를 비활성화시킴
                    ui.SetActive(false);
                }
                else if (!ui.activeSelf)
                {
                    Debug.Log("나가기 UI 활성화");
                }
            }
        }

    }
}
