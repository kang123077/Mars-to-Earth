
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image skillImage;
    public Image isEnforce;
    public Image coolDown;

    public Skill.Skill skill;
    public void Init(Skill.Skill inputSkill)
    {
        skill = inputSkill;
        skillImage.sprite = skill.skillInfo.icon;
        skillImage.enabled = true;
    }

    private void Update()
    {
        if(!skill)return;
        if (skill.isCombo)
        {
            coolDown.fillAmount= (Time.time-skill.lastUsedTime) / skill.curCoolTime;
            if (coolDown.fillAmount >= 1)
                skill.isCombo = false;
        }
        else
            coolDown.fillAmount= (skill.lastUsedTime+skill.curCoolTime -Time.time) / skill.curCoolTime;
            
    }
}
