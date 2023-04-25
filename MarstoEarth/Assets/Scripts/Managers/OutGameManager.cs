using UnityEngine.SceneManagement;
using UnityEngine;

public class OutGameManager : Singleton<OutGameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void SceneChage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
