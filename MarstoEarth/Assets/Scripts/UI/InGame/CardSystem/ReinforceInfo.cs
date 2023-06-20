using UnityEngine;
using UnityEngine.UI;

public class ReinforceInfo : MonoBehaviour
{
    public Image reinIcon;
    public TMPro.TextMeshProUGUI reinText;
    public CombatUI combatInfo;
    SkillSlot[] loadSkills;
    int checkIndex;
    public TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        loadSkills = new SkillSlot[4];
    }

    public void ReinforceAdd()
    {
        checkIndex = combatInfo.EnforceSkill();
        if (checkIndex == -1)
            return;
        if (CombatUI.fullCheck == true)
        {
            for (int i = 0; i < combatInfo.skillSlots.Length; i++)
            {
                loadSkills[i] = combatInfo.skillSlots[i];
            }
        }
        reinIcon.sprite = loadSkills[checkIndex].skill.skillInfo.icon;
        reinText.text = loadSkills[checkIndex].skill.skillInfo.name + "\n\n" + loadSkills[checkIndex].skill.skillInfo.description2;
    }

    public void ReinforceStop()
    {
        MapInfo.pauseRequest++;
        text.gameObject.SetActive(true);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            MapInfo.pauseRequest--;
        }
    }
}
