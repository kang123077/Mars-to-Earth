using Character;
using Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CombatUI : UI
{
    public SkillSlot[] skillSlots;
    public Transform[] mSlotPos;

    private int _curSkillCount;

    private int curSkillCount
    {
        get => _curSkillCount;
        set
        {
            if (value >= skillSlots.Length)
                fullCheck = true;
            else
                _curSkillCount = value;
        }
    }
    public UnityEngine.UI.Slider mplayerHP;
    public UnityEngine.UI.Slider playerHP;
    public UnityEngine.UI.Image hitScreen;
    public IObjectPool<DamageText> DMGTextPool;
    public DamageText DMGText;

    public RectTransform SPCSlotsTransform;
    public Transform MSPCSlotsPos;

    public List<UnityEngine.UI.Image> SPCSlots = new();
    public UnityEngine.UI.Image SPCPrefab;
    public GameObject[] PCMO;

    public JoyStick MovingPad;
    public RectTransform Shot;
    public RectTransform Pause;
    public RectTransform Dodge;

    public static bool fullCheck;

    private void Awake()
    {
        DMGTextPool = new ObjectPool<DamageText>(() =>
        {
            DamageText copyPrefab = Instantiate(DMGText, transform);
            copyPrefab.gameObject.SetActive(false);
            return copyPrefab;
        }, actionOnRelease: (dt) => dt.gameObject.SetActive(false), defaultCapacity: 20, maxSize: 40);

#if UNITY_ANDROID || UNITY_IOS
        PCMO[0].SetActive(false);
        PCMO[1].SetActive(true);
        playerHP = mplayerHP;
        for (int i = 0; i < skillSlots.Length; i++)
            skillSlots[i].transform.position = mSlotPos[i].position;
        SPCSlotsTransform.position= MSPCSlotsPos.position;


#else
        PCMO[0].SetActive(true);
        PCMO[1].SetActive(false);
#endif
    }

    public void ConnectSPCImage(Sprite icon)
    {
        UnityEngine.UI.Image spcClone = Instantiate(SPCPrefab, SPCSlotsTransform);
        spcClone.sprite = icon;
        spcClone.gameObject.SetActive(true);
        SPCSlots.Add(spcClone);
    }

    public void LearnSkill(int skillName)
    {
        if (curSkillCount > skillSlots.Length)
        {
            for (int i = 0; i < skillSlots.Length; i++)
            {
                if (skillSlots[i].skill == InGameManager.Instance.inGameSkill[skillName])
                {
                    skillSlots[i].skill.enforce =true;
                }
            }
            return;
        }
        skillSlots[curSkillCount].Init(InGameManager.Instance.inGameSkill[skillName]);
        skillSlots[curSkillCount].skill.Init(SpawnManager.Instance.player);
        curSkillCount++;
    }
    public void ClickSkill(int idx)
    {
        if (curSkillCount <= idx) return;
        SkillSlot slot = skillSlots[idx];

        if ((!slot.skill.isCombo && slot.coolDown.fillAmount <= 0) ||
           (slot.skill.isCombo) || (SpawnManager.Instance.player.onSkill is MassShootingSkill))
        {
            slot.skill.Use();
        }
    }

#if  UNITY_ANDROID || UNITY_IOS 
    int MovingPadId = -1;
    int sightId = -1;
    float lastInputTime;


    private void Update()
    {
        if (Input.touchCount > 0)
        {           
            foreach (var touch in Input.touches)
            {                
                if (touch.phase == TouchPhase.Began)
                {                    
                    if (RectTransformUtility.RectangleContainsScreenPoint(Shot,touch.position))
                    {
                        SpawnManager.Instance.player.Attacken();
                    }
                    else if (RectTransformUtility.RectangleContainsScreenPoint(Pause, touch.position))
                    {
                        UIManager.Instance.UIs[(int)UIType.Setting].gameObject.SetActive(true);
                    }else if (RectTransformUtility.RectangleContainsScreenPoint(Dodge, touch.position))
                    {
                        SpawnManager.Instance.player.actives[0].Use();
                    }
                    else if (touch.position.x< Screen.width*0.3f)
                    {
                        if (MovingPadId != -1) return;
                        MovingPad.transform.position= touch.position;
                        MovingPad.gameObject.SetActive(true);                      
                        MovingPadId =touch.fingerId;
                        //MovingPad.OnDrag(touch.position);
                        if (Time.time - lastInputTime < 0.3f)
                        {
                            SpawnManager.Instance.player.isRun = true;
                        }
                        lastInputTime = Time.time;
                    }
                    else
                    {
                        
                        sightId=touch.fingerId;
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {                   

                    if (MovingPadId == touch.fingerId)
                    {
                        MovingPad.OnDrag(touch.position);

                    }
                    else if (sightId == touch.fingerId)
                    {
                        CinemachineManager.Instance.curAngle.y += touch.deltaPosition.x*Time.deltaTime*3;
                        CinemachineManager.Instance.follower.rotation = Quaternion.Euler(CinemachineManager.Instance.curAngle);
                    }
                }
                else if(touch.phase==TouchPhase.Ended)
                {
                    if (MovingPadId==touch.fingerId)
                    {
                       
                        MovingPadId = -1;
                        SpawnManager.Instance.player.xInput = 0;
                        SpawnManager.Instance.player.zInput = 0;
                        SpawnManager.Instance.player.isRun = false; 
                        MovingPad.gameObject.SetActive(false);                        
                    }else if (sightId == touch.fingerId)
                    {
                        sightId = -1;
                    }
                }
                
            }
        }
    }
    
#endif
}
