using UnityEngine;

public class OutGameUIManager : Singleton<OutGameUIManager>
{
    public int recordClearRoom;
    public GameObject reportUI;
    public GameObject explainUI;
    public GameObject settingUI;
    public AudioClip[] _BGM_AudioClips;
    public AudioClip[] _UI_EffectAudioClips;

    protected override void Awake()
    {
        AudioManager.BGM_AudioClips = _BGM_AudioClips;
        AudioManager.UI_EffectAudioClips = _UI_EffectAudioClips;
        reportUI.SetActive(false);
        explainUI.SetActive(false);
        settingUI.SetActive(false);
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


        }
        
    }
}
