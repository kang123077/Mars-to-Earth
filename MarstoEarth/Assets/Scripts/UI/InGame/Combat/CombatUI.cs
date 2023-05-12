using Character;
using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CombatUI : UI
{
    public SkillSlot[] skillSlots;
    private int curSkillCount;
    
    
    public UnityEngine.UI.Slider mplayerHP;
    
    public UnityEngine.UI.Slider playerHP;
    
    public UnityEngine.UI.Image hitScreen;
    public IObjectPool<DamageText> DMGTextPool;
    public DamageText DMGText;

    public RectTransform SPCSlotsTransform;
    public List<UnityEngine.UI.Image> SPCSlots = new();
    public UnityEngine.UI.Image SPCPrefab;
    public GameObject[] PCMO;
    

    private void Awake()
    {
        DMGTextPool = new ObjectPool<DamageText>(() =>
        {
            DamageText copyPrefab = Instantiate(DMGText,transform);
            copyPrefab.gameObject.SetActive(false);
            return copyPrefab;
        }, actionOnRelease: (dt) => dt.gameObject.SetActive(false), defaultCapacity: 20, maxSize: 40);
        
        #if UNITY_ANDROID||UNITY_IOS||UNITY_EDITOR
            PCMO[0].SetActive(false);
            PCMO[1].SetActive(true);
            playerHP = mplayerHP;
        #else
            PCMO[0].SetActive(true);
            PCMO[1].SetActive(false);
        #endif
    }

    public void ConnectSPCImage(Sprite icon)
    {
        UnityEngine.UI.Image spcClone = Instantiate(SPCPrefab,SPCSlotsTransform);
        spcClone.sprite = icon;
        spcClone.gameObject.SetActive(true); 
        SPCSlots.Add(spcClone);
    }

    public void LearnSkill(int skillName)
    {
        if (curSkillCount > skillSlots.Length - 1) return;
        skillSlots[curSkillCount].Init(InGameManager.Instance.inGameSkill[(int)skillName]);
        skillSlots[curSkillCount].skill.Init(SpawnManager.Instance.player);
        curSkillCount++;
    }
    public void ClickSkill(int idx)
    {
        if (curSkillCount <= idx) return;
        SkillSlot slot = skillSlots[idx];

        if((!slot.skill.isCombo&&slot.coolDown.fillAmount<=0)||
           ( slot.skill.isCombo)|| (SpawnManager.Instance.player.onSkill is MassShootingSkill ))
        {
            slot.skill.Use();
        }
    }

}
