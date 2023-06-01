using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public System.Collections.Generic.List<Sprite> imgBackGround;
    public Image bgi;
    public Slider progressBar;
    public Slider mprogressBar;
    public TMPro.TMP_Text loadingText;
    public TMPro.TMP_Text mloadingText;
    private System.Random random;
    public GameObject[] pcEXmo;

    private void Start()
    {
        StartCoroutine(LoadScene());
        random = new System.Random();

        int randomIndex = random.Next(0, imgBackGround.Count);
        Sprite randomSprite = imgBackGround[randomIndex];
        bgi.sprite = randomSprite;

#if UNITY_ANDROID || UNITY_IOS
        pcEXmo[0].SetActive(false);
        pcEXmo[1].SetActive(true);


#else
        pcEXmo[0].SetActive(true);
        pcEXmo[1].SetActive(false);
#endif
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("InGameScene");
        // operation.isDone;   // 작업 완료 유무 boolean형 반환
        // operation.progress; // 진행 정도를 floa형 0, 1을 반환(0:진행중, 1:진행완)
        operation.allowSceneActivation = false; // true시 로딩 완료면 씬을 넘김 false면 progress가 0.9f에서 멈춤 true가 될 때까지 기다림
        while (!operation.isDone)    // 로딩이 끝나 isDone이 true가 되기 전까지 계속 반복
        {
            yield return null;
#if UNITY_ANDROID || UNITY_IOS
            if (mprogressBar.value < 0.9f)
            {
                mprogressBar.value = Mathf.MoveTowards(mprogressBar.value, 0.9f, Time.deltaTime);
                mloadingText.text = "로딩 중..";
            }
            else if (operation.progress >= 0.9f)
            {
                mprogressBar.value = Mathf.MoveTowards(mprogressBar.value, 1f, Time.deltaTime);
                mloadingText.text = "로딩 중.";
            }
            if(mprogressBar.value >= 1f)
            {
                mloadingText.text = "화면을 터치해주세요!";
            }
            if (Input.touchCount == 1f || Input.GetMouseButton(0) && mprogressBar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
#else
            if (progressBar.value < 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f, Time.deltaTime);
                loadingText.text = "로딩 중..";
            }
            else if (operation.progress >= 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
                loadingText.text = "로딩 중.";
            }
            if (progressBar.value >= 1f)
            {
                loadingText.text = "Spacebar를 누르세요!";
            }
            if (Input.GetKeyDown(KeyCode.Space) && progressBar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
#endif
        }
    }
}
