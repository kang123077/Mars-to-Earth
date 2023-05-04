using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InGameScene : MonoBehaviour
{
    public LoadingScene loadingScene;

    public void LoadGameScene()
    {
        StartCoroutine(loadingScene.LoadSceneAsync("InGameScene"));
    }
}

