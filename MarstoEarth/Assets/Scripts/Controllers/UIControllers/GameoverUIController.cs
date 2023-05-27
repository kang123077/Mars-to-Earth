using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverUIController : MonoBehaviour
{
    public ReportContentUIController reportContent;

    private void OnEnable()
    {
        InitReportContent();
    }

    public void GotoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OutGameScene");
        gameObject.SetActive(false);
    }

    public void InitReportContent()
    {
        reportContent.InitContent();
    }
}
