using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UIType
{
    Combat,
    Card,
    MiniMap,
    Setting,
}

public class UIManager : Singleton<UIManager>
{
    public UI[] UIs;

    private Stack<UI> uiStack = new Stack<UI>();
    private UI currentView;
    public StageClearUIController stageClearUI;
    public GameoverUIController gameoverUI;
    public TMP_InputField inputField;
    public TMP_Text timeUI;
    public RectTransform aimImage;
    public Transform muzzleTr;
    public Transform lookAtTr;

    protected override void Awake()
    {
        base.Awake();
        try
        {
            // stageClearUI.gameObject.SetActive(false);
            // gameoverUI.gameObject.SetActive(false);
        }
        catch (NullReferenceException)
        {
            // 씬에 StageClearUI or GameoverUI가 없거나 UIManager에 등록하지 않음
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        currentView = UIs[(int)UIType.Combat];
    }

    private void Update()
    {
        int minutes = Mathf.FloorToInt(MapInfo.cur_Time / 60);
        int seconds = Mathf.FloorToInt(MapInfo.cur_Time % 60);

        string formattedTime = minutes.ToString("00") + " : " + seconds.ToString("00");
        timeUI.text = formattedTime;
        // Esc 버튼 클릭 시 소리를 끄고 Setting UI를 활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlayEffect(1);
            AudioManager.Instance.PauseSource();
            aimImage.gameObject.SetActive(false);
            if (UIs[(int)UIType.Setting].gameObject.activeSelf != true)
            {
                UIs[(int)UIType.Setting].gameObject.SetActive(true); // UI 활성화
                Time.timeScale = 0f;
            }
            else if (UIs[(int)UIType.Setting].gameObject.activeSelf == true)
            {
                if (UIs[(int)UIType.Card].gameObject.activeSelf == true)
                {
                    Time.timeScale = 0f;
                }
                else if (UIs[(int)UIType.Card].gameObject.activeSelf != true)
                {
                    AudioManager.Instance.UnPauseSorce();
                    Time.timeScale = 1f;
                    aimImage.gameObject.SetActive(true);
                }
                UIs[(int)UIType.Setting].gameObject.SetActive(false); // UI 활성화
            }
        }

        // 시네 카메라 활성화에 따른 위치값을 받음 anchorMin과 anchorMax에 해당하는 위치를 저장해 어느 해상도에도 반응이 되게 설정함
        if (CinemachineManager.Instance.playerCam.gameObject.activeSelf)
        {
            muzzleTr = SpawnManager.Instance.player.muzzle.transform;
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(muzzleTr.position);
            aimImage.anchorMin = viewportPos;
            aimImage.anchorMax = viewportPos;
        }
        else if (CinemachineManager.Instance.bossCam.gameObject.activeSelf)
        {
            if (CinemachineManager.Instance.bossCam.LookAt != null)
            {
                lookAtTr = CinemachineManager.Instance.bossCam.LookAt.transform;
                Vector2 viewportPos = Camera.main.WorldToViewportPoint(lookAtTr.position) + new Vector3(0f, 0.1f);
                aimImage.anchorMin = viewportPos;
                aimImage.anchorMax = viewportPos;
                aimImage.localScale = new Vector3(1.3f, 1.3f);
            }
        }
    }

    public void ShowUI(UIType uiType)
    {
        PushUIView(currentView);
        UIs[(int)uiType].Show();
        currentView = UIs[(int)uiType];
    }

    public void PushUIView(UI view)
    {
        view.Close();
        uiStack.Push(view);
    }

    public void StageClear()
    {
        Time.timeScale = 0f;
        stageClearUI.gameObject.SetActive(true);
    }

    public void Gameover()
    {
        Time.timeScale = 0f;
        gameoverUI.gameObject.SetActive(true);
    }

    public void PopUIView()
    {
        if (uiStack.Count <= 0) return;

        currentView.Close();
        currentView = uiStack.Pop();
        currentView.Show();
    }

    public void RequestChangeSeedNumber()
    {
        // MapSeedNum UI에서 사용
        MapManager.Instance.ChangeSeedNumber(inputField.text);
    }
}

