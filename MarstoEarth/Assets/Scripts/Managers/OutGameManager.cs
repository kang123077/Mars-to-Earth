
using UnityEngine.SceneManagement;

public class OutGameManager : Singleton<OutGameManager>
{
    protected override void Awake()
    {
        base.Awake();
        OutGameInit();
    }

    public void SceneChage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OutGameInit()
    {
        MapInfo.ResetValues();        
        Character.staticStat.ResetValues(SpawnManager.Instance.player);
    }
}
