using UnityEngine.SceneManagement;
using UnityEngine;

public class OutGameManager : Singleton<OutGameManager>
{
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
}
