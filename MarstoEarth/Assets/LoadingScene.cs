using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingUI;
    public Image loadingBar;

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        // 로딩 UI를 활성화합니다.
        ShowLoadingUI();

        // 다음 씬을 비동기적으로 로드합니다.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // 로딩이 완료될 때까지 대기합니다.
        while (!asyncOperation.isDone)
        {
            // 로딩 바 UI를 업데이트합니다.
            UpdateLoadingBar(asyncOperation.progress);

            yield return null;
        }

        // 로딩 UI를 비활성화합니다.
        HideLoadingUI();
    }

    private void ShowLoadingUI()
    {
        loadingUI.SetActive(true);
    }

    private void UpdateLoadingBar(float progress)
    {
        loadingBar.fillAmount = progress;
    }

    private void HideLoadingUI()
    {
        loadingUI.SetActive(false);
    }
}
