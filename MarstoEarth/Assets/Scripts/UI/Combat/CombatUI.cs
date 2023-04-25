
using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : UI
{
    public SkillSlot[] skillSlots;
    public UnityEngine.UI.Slider playerHP;

    public UnityEngine.UI.Image hitScreen;
    private int curSkillCount;

    public void LearnSkill(int skillName)
    {
        
        if (curSkillCount > skillSlots.Length - 1) return;
        skillSlots[curSkillCount].Init(ResourceManager.Instance.skills[(int)skillName]);
        curSkillCount++;
        
    }
    public void ClickSkill(int idx)
    {
        if (curSkillCount <= idx) return;

        SkillSlot slot = skillSlots[idx];
        if(!slot.skill.isCombo&&slot.coolDown.fillAmount<=0||slot.skill.isCombo)
            slot.skill.Use(SpawnManager.Instance.player);
    }

}
