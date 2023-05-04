using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    public Slider maserVolume;
    public Slider BGMVolume;
    public Slider effectVolume;
    private Dropdown resolutionCon;

    void Start()
    {
        resolutionCon = GetComponentInChildren<Dropdown>();
        gameObject.SetActive(false); // UI 비활성화
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            gameObject.SetActive(true); // UI 활성화
        }
    }

    public void OffSettingUI()
    {
        gameObject.SetActive(false);
    }
}
