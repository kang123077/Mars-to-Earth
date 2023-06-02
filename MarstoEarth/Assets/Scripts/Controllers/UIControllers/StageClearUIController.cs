using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearUIController : MonoBehaviour
{
    public void NextStage()
    {
        MapInfo.pauseRequest--;
        SceneManager.LoadScene("LoadingScene");
        gameObject.SetActive(false);
    }
}
