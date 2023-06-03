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
                    // Debug.Log("나가기 UI 활성화");
                }
            }
        }

    }
}
