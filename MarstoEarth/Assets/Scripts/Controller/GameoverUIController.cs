using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverUIController : MonoBehaviour
{
    public void GotoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OutGameScene");
        gameObject.SetActive(false);
    }
    public void Retry()
    {
        MapInfo.isRetry = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoadingScene");
        gameObject.SetActive(false);
    }
}
