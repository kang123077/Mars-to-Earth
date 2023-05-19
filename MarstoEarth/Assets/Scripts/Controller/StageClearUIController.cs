using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearUIController : MonoBehaviour
{
    public void NextStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoadingScene");
        gameObject.SetActive(false);
    }
}
