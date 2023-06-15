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
    MobileSetting,
}

public class UIManager : Singleton<UIManager>
{
    public UI[] UIs;
    bool isPaused = false;

    private Stack<UI> uiStack = new Stack<UI>();
    private UI currentView;
    public StageClearUIController stageClearUI;
    public GameoverUIController gameoverUI;
    public GameInfoUIController gameInfoUIController;
    public PlayerStatUIController playerStatUIController;
    public TMP_InputField inputField;
    public UnityEngine.UI.Image aimSprite;
    public Sprite[] spriteArray;
    public RectTransform aimImage;
    public Transform muzzleTr;
    public Transform lookAtTr;
    public GameObject[] PCMO;


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
        Cursor.lockState = CursorLockMode.Confined;
        currentView = UIs[(int)UIType.Combat];
        playerStatUIController.core = MapInfo.core;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;
            UIs[(int)UIType.Setting].gameObject.SetActive(true);
        }
        else
        {
            if (pause)
            {
                isPaused = false;
            }
        }
        isPaused = pause;
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS

    
#else

        // Esc 버튼 클릭 시 소리를 끄고 Setting UI를 활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlayEffect(1);
            AudioManager.Instance.PauseSource();
            aimImage.gameObject.SetActive(false);
            if (UIs[(int)UIType.Setting].gameObject.activeSelf != true)
            {
                UIs[(int)UIType.Setting].gameObject.SetActive(true); // UI 활성화
                MapInfo.pauseRequest++;
            }
            else if (UIs[(int)UIType.Setting].gameObject.activeSelf == true)
            {
                AudioManager.Instance.UnPauseSorce();
                MapInfo.pauseRequest--;
                aimImage.gameObject.SetActive(true);
                UIs[(int)UIType.Setting].gameObject.SetActive(false); // UI 비활성화
            }
        }
#endif


        // 시네 카메라 활성화에 따른 위치값을 받음 anchorMin과 anchorMax에 해당하는 위치를 저장해 어느 해상도에도 반응이 되게 설정함
        if (CinemachineManager.Instance.playerCam.gameObject.activeSelf)
        {
            muzzleTr = SpawnManager.Instance.player.muzzle.transform;
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(muzzleTr.position);
            // 보간을 사용하여 현재 위치에서 목표 위치로 부드럽게 이동합니다.
            Vector2 currentAnchorMin = aimImage.anchorMin;
            Vector2 currentAnchorMax = aimImage.anchorMax;
            float smoothness = 15f; // 조절 가능한 매끄러움 정도입니다. 0에 가까울수록 부드럽게 이동합니다.
            aimImage.anchorMin = Vector2.Lerp(currentAnchorMin, viewportPos, smoothness * Time.deltaTime);
            aimImage.anchorMax = Vector2.Lerp(currentAnchorMax, viewportPos, smoothness * Time.deltaTime);
            aimSprite.sprite = spriteArray[0];
            aimSprite.color = Color.clear;
        }
        else if (CinemachineManager.Instance.bossCam.gameObject.activeSelf)
        {
            if (CinemachineManager.Instance.bossCam.LookAt != null)
            {
                lookAtTr = CinemachineManager.Instance.bossCam.LookAt.transform;
                Vector2 viewportPos = Camera.main.WorldToViewportPoint(lookAtTr.position) + new Vector3(0f, 0.1f);
                // 보간을 사용하여 현재 위치에서 목표 위치로 부드럽게 이동합니다.
                Vector2 currentAnchorMin = aimImage.anchorMin;
                Vector2 currentAnchorMax = aimImage.anchorMax;
                Vector3 currentScale = aimImage.localScale;

                float smoothness = 15f; // 조절 가능한 매끄러움 정도입니다. 0에 가까울수록 부드럽게 이동합니다.
                aimSprite.sprite = spriteArray[1];
                aimSprite.color = Color.green;
                aimImage.anchorMin = Vector2.Lerp(currentAnchorMin, viewportPos, smoothness * Time.deltaTime);
                aimImage.anchorMax = Vector2.Lerp(currentAnchorMax, viewportPos, smoothness * Time.deltaTime);
                aimImage.localScale = Vector3.Lerp(currentScale, new Vector3(1.3f, 1.3f), smoothness * Time.deltaTime);
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
        MapInfo.pauseRequest++;
        stageClearUI.gameObject.SetActive(true);
    }

    public void Gameover()
    {
        MapInfo.pauseRequest++;
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

