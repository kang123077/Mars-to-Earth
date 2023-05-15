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
                }
                UIs[(int)UIType.Setting].gameObject.SetActive(false); // UI 활성화
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

