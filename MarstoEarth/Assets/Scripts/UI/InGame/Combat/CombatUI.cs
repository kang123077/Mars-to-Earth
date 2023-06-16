using Character;
using Skill;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
    private int _curEnforceSkillCount;

    private int curEnforceSkillCount
    {
        get => _curEnforceSkillCount;
        set
        {
            if (value >= skillSlots.Length)
                enforceFullCheck = true;
            else
                _curEnforceSkillCount = value;
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
    public static bool enforceFullCheck;
    public static Player player;

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
        SPCSlotsTransform.position = MSPCSlotsPos.position;


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
                    skillSlots[i].skill.enforce = true;
                }
            }
            return;
        }
        skillSlots[curSkillCount].Init(InGameManager.Instance.inGameSkill[skillName]);
        skillSlots[curSkillCount].skill.Init(player);
        curSkillCount++;
    }

    public int EnforceSkill()
    {
        if (enforceFullCheck) return -1;
        int randIdx;
        do
        {
            randIdx = SpawnManager.rand.Next(0, 4);
        } while (skillSlots[randIdx].skill.enforce);
        skillSlots[randIdx].skill.enforce = true;
        skillSlots[randIdx].isEnforce.gameObject.SetActive(true);
        curEnforceSkillCount++;
        return randIdx;
    }
    public void ClickSkill(int idx)
    {
        if (curSkillCount < idx) return;
        SkillSlot slot = skillSlots[idx];
        if (slot.skill is null) return;

        if ((!slot.skill.isCombo && slot.coolDown.fillAmount <= 0) ||
           (slot.skill.isCombo) || (player.onSkill is MassShootingSkill))
        {
            slot.skill.Use();
        }

    }
    private void Start()
    {
        player = SpawnManager.Instance.player;
    }
#if  UNITY_ANDROID || UNITY_IOS 
    int MovingPadId = -1;
    int sightId = -1;
    float lastInputTime;
    float beganPoint = 0;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (RectTransformUtility.RectangleContainsScreenPoint(Shot, touch.position)
                       && player.isRun == false)
                        {
                            sightId = touch.fingerId;
                            beganPoint = touch.position.x;
                            if (player.onSkill is not null && player.onSkill.skillInfo.clipLayer == 2)
                                return;
                            player.anim.SetTrigger(Character.Character.attacking);
                        }
                        else if (RectTransformUtility.RectangleContainsScreenPoint(Pause, touch.position))
                        {
                            if (UIManager.Instance.UIs[(int)UIType.MobileSetting].gameObject.activeSelf) break;                            
                            UIManager.Instance.UIs[(int)UIType.MobileSetting].gameObject.SetActive(true);
                            MapInfo.pauseRequest++;
                        }
                        else if (RectTransformUtility.RectangleContainsScreenPoint(Dodge, touch.position))
                        {
                            if (player.onSkill is not null && player.onSkill.skillInfo.clipLayer == 2)
                                return;
                           
                            
                            player.actives[0].Use();
                        }
                        else if (touch.position.x < Screen.width * 0.3f)
                        {
                            if (MovingPadId != -1) break;
                            MovingPad.transform.position = touch.position;
                            MovingPad.gameObject.SetActive(true);
                            MovingPadId = touch.fingerId;
                            MovingPad.OnDrag(touch.position);
                            if (Time.time - lastInputTime < 0.3f)                   
                                SpawnManager.Instance.player.isRun = true;
                            
                            lastInputTime = Time.time;
                        }
                        else
                        {
                            sightId = touch.fingerId;
                            beganPoint = touch.position.x;
                        }
                        break;
                    case TouchPhase.Moved:
                        if (MovingPadId == touch.fingerId && MovingPadId != -1)
                            MovingPad.OnDrag(touch.position);

                        else if (sightId == touch.fingerId)
                        {
                            CinemachineManager.Instance.curAngle.y += (touch.position.x -beganPoint) * Time.deltaTime *1.5f;
                            CinemachineManager.Instance.follower.rotation = Quaternion.Euler(CinemachineManager.Instance.curAngle);
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (sightId == touch.fingerId)
                        {
                            CinemachineManager.Instance.curAngle.y += (touch.position.x - beganPoint) * Time.deltaTime * 1.5f;
                            CinemachineManager.Instance.follower.rotation = Quaternion.Euler(CinemachineManager.Instance.curAngle);
                        }
                        break;
                    case TouchPhase.Ended:
                        if (MovingPadId == touch.fingerId)
                        {
                            MovingPadId = -1;
                            player.xInput = 0;
                            player.zInput = 0;
                            player.InputDir = Vector3.zero;
                            player.isRun = false;
                            MovingPad.gameObject.SetActive(false);
                        }
                        else if (sightId == touch.fingerId)
                            sightId = -1;
                        break;                  
                }
            }
        }
    }

#endif
}
