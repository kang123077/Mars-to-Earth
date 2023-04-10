using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour
{
    public Image icon;
    public Image cool;
    public float coolTime;
    float elapsed;
    bool bUserSkill;
    void Start()
    {
        bUserSkill = false;
        cool.fillAmount = 0f;
        elapsed = coolTime;
    }
    public void ChageIten(string iconName)
    {
        icon.sprite = Resources.Load<Sprite>(iconName);
        icon.gameObject.SetActive(true);
    }
    public void ResetItem()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        elapsed = 0f;
    }
    public void UseSkill()
    {
        Debug.Log("스킬 사용!");
        bUserSkill = true;
        cool.fillAmount = 1f;
    }
    public void OnPointerClick(BaseEventData eventData)
    {
        if (bUserSkill)
        {
            return;
        }
        if (icon.sprite != null && icon.gameObject.activeSelf == true)
        {
            UseSkill();
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            cool.fillAmount = 1;
            if (cool.fillAmount < 1f)
            {
                Debug.Log("스킬을 사용할 수 없습니다");
            }
        }
        cool.fillAmount -= 0.005f;

        //if (bUserSkill)
        //{
        //    elapsed = elapsed - Time.deltaTime;
        //    cool.fillAmount = elapsed / coolTime;
        //    if (cool.fillAmount <= 0f)
        //    {
        //        bUserSkill = false;
        //        elapsed = coolTime;
        //        Debug.Log("쿨타임이 종료되었을 경우");
        //    }
        //}
    }
}
