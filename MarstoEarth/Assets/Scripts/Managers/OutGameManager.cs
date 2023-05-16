using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class OutGameManager : Singleton<OutGameManager>
{
    public Image blinkImage;

    protected override void Awake()
    {
        base.Awake();
        OutGameInit();
    }

    public void SceneChage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OutGameInit()
    {
        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            Destroy(mapManager.gameObject);
        }
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
