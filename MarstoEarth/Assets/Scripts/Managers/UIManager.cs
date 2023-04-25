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

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //currentView = UIs[(int)UIType.Combat];
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

