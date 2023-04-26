
using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : UI
{
    public SkillSlot[] skillSlots;
    private int curSkillCount;

    public UnityEngine.UI.Slider playerHP;
    public UnityEngine.UI.Image hitScreen;


    public RectTransform SPCSlotsTransform;
    public List<UnityEngine.UI.Image> SPCSlots = new();
    public UnityEngine.UI.Image SPCPrefab;

    public void ConnectSPCImage(Sprite icon)
    {
        UnityEngine.UI.Image spcClone = Instantiate(SPCPrefab, SPCSlotsTransform);
        spcClone.gameObject.SetActive(true); 
        SPCSlots.Add(spcClone);
        Debug.Log(SPCSlots.Count);
    }



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
