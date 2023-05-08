using System.Collections;
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

    bool isBool;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        currentView = UIs[(int)UIType.Combat];
        AudioManager.Instance.PlayBGM(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlayEffect(2);
            if (UIs[(int)UIType.Setting].gameObject.activeSelf != true)
            {
                UIs[(int)UIType.Setting].gameObject.SetActive(true); // UI 활성화
                Time.timeScale = 0f;
            }
            else if (UIs[(int)UIType.Setting].gameObject.activeSelf == true)
            {
                UIs[(int)UIType.Setting].gameObject.SetActive(false); // UI 활성화
                Time.timeScale = 1f;
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

