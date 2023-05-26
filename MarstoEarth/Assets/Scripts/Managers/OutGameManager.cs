
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
        CombatUI.fullCheck = false;
        CombatUI.enforceFullCheck = false;
        Character.staticStat.ResetValues(SpawnManager.Instance.player);
    }
}
