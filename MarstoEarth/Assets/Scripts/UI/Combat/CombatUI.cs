using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : UI
{
    public SkillSlot[] skillSlots;
    public UnityEngine.UI.Slider playerHP;
    private UnityEngine.UI.Image image;
    private int curSkillCount=0;
    private int i = 0;
    void Start()
    {
        
    }

    private void Update()
    {
        for(i=0; i<curSkillCount; i++)
        {
            
        }
    }

    public void LearnSkill(SkillName skillName)
    {
        if (curSkillCount > skillSlots.Length - 1) return;
        skillSlots[curSkillCount].skill = ResourceManager.Instance.skills[(int)skillName];
        skillSlots[curSkillCount].TryGetComponent(out image);
        image.raycastTarget = true;
        

        curSkillCount++;

    }
    public void ClickSkill(int idx)
    {
        if (curSkillCount <= idx) return;
        skillSlots[idx].skill.Use(SpawnManager.Instance.player);
    }

}
