using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class OutGameManager : Singleton<OutGameManager>
{
    public Image blinkImage;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SceneChage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
