using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class OutGameManager : Singleton<OutGameManager>
{
    public string URL = ""; // 버전체크를 위한 URL
    public string CurVersion; // 현재 빌드버전
    string latsetVersion; // 최신버전
    public GameObject newVersionAvailable; // 버전확인 UI
    public GameObject internetCheck; // 인터넷 연결 UI

    protected override void Awake()
    {
        base.Awake();
        OutGameInit();
    }

    void Start()
    {
        StartCoroutine(LoadTxtData(URL));
    }

    public void SceneChage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OutGameInit()
    {
        MapInfo.ResetValues();
        Character.staticStat.ResetValues();
    }

    public void VersionCheck()
    {
        if (CurVersion != latsetVersion)
        {
            newVersionAvailable.SetActive(true);
        }
        else
        {
            newVersionAvailable.SetActive(false);
        }
    }


    IEnumerator LoadTxtData(string url)
    {
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(url);
        yield return www.SendWebRequest(); // 페이지 요청
       
        if (www.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || www.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error get page");
            internetCheck.SetActive(true);
        }
        else
        {
            latsetVersion = www.downloadHandler.text; // 웹에 입력된 최신버전
            internetCheck.SetActive(false);
            VersionCheck();
        }
    }

    public void OpenURL(string url) // 스토어 열기
    {
        Application.OpenURL(url);
    }
}
