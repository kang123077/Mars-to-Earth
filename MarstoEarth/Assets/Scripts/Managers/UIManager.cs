using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager :Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
