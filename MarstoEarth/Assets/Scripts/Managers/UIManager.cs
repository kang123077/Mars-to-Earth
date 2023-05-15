using System.Collections.Generic;
using UnityEngine;


public enum UIType

{
    Combat,
    Card,
    MiniMap,
    Setting,
}
public class UIManager :Singleton<UIManager>
{
    public UI[] UIs;
   
    private Stack<UI> uiStack = new Stack<UI>();
    private UI currentView;
    public RectTransform aimImage;
    public Transform muzzleTr;
    public Transform lookAtTr;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        currentView = UIs[(int)UIType.Combat];
    }

    private void Update()
    {
        // Time.timeScale = 0f일 때 소리를 끄게끔
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
                if(UIs[(int)UIType.Card].gameObject.activeSelf == true)
                {
                    Time.timeScale = 0f;
                }
                else if(UIs[(int)UIType.Card].gameObject.activeSelf != true)
                {
                    AudioManager.Instance.UnPauseSorce();
                    Time.timeScale = 1f;
                    aimImage.gameObject.SetActive(true);
                }
                UIs[(int)UIType.Setting].gameObject.SetActive(false); // UI 활성화
            }
        }

        if(CinemachineManager.Instance.playerCam.gameObject.activeSelf == true)
        {
            muzzleTr = SpawnManager.Instance.player.muzzle.transform;
            aimImage.anchoredPosition = Camera.main.WorldToScreenPoint(muzzleTr.position);
        }
        else if(CinemachineManager.Instance.bossCam.gameObject.activeSelf == true)
        {
            if (CinemachineManager.Instance.bossCam.LookAt != null)
            {
                lookAtTr = CinemachineManager.Instance.bossCam.LookAt.transform;
                lookAtTr.localScale = new Vector3(1.3f, 1.3f);
                aimImage.anchoredPosition = Camera.main.WorldToScreenPoint(lookAtTr.position);
            }
            else
            {
                Debug.Log("실행중");
                lookAtTr = null;
                aimImage.anchoredPosition = Vector2.zero;
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
    public void PopUIView()
    {
        if (uiStack.Count <= 0) return;

        currentView.Close();
        currentView = uiStack.Pop();
        currentView.Show();
    }
}

