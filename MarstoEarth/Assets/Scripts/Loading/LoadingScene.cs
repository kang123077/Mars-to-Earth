using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image[] backGroundImage;
    public Slider progressBar;
    public TMPro.TMP_Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("InGameSceneUIPractice");
        // operation.isDone;   // 작업 완료 유무 boolean형 반환
        // operation.progress; // 진행 정도를 floa형 0, 1을 반환(0:진행중, 1:진행완)
        operation.allowSceneActivation = false; // true시 로딩 완료면 씬을 넘김 false면 progress가 0.9f에서 멈춤 true가 될 때까지 기다림
        
        while(!operation.isDone)    // 로딩이 끝나 isDone이 true가 되기 전까지 계속 반복
        {
            yield return null;
            if(progressBar.value < 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f, Time.deltaTime);
            }
            else if(operation.progress >= 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            }

            if (progressBar.value >= 1f)
            {
                loadingText.text = "Press SpaceBar";
            }

            if(Input.GetKeyDown(KeyCode.Space) && progressBar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                Debug.Log(operation.isDone);
            }
        }
    }
}
