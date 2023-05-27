using UnityEngine;

public class OutGameUIManager : Singleton<OutGameUIManager>
{
    public int recordClearRoom;
    public GameObject reportUI;
    public GameObject explainUI;
    public GameObject settingUI;

    protected override void Awake()
    {
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
