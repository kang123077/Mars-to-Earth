using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearUIController : MonoBehaviour
{
    public void GotoNextStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoadingScene");
        gameObject.SetActive(false);
        // MapInfo 명령 들어갈 곳
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoadingScene");
        gameObject.SetActive(false);
    }
}
