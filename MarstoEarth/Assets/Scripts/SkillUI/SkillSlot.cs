using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillSlot : MonoBehaviour
{
    //public Image iconImage; // 스킬 아이콘 이미지
    //public Text cooldownText; // 쿨다운 텍스트

    //private bool isCoolingDown = false; // 쿨다운 중인지 여부

    //// 스킬 아이콘과 쿨다운 시간을 설정하는 함수
    //public void SetSkill(Sprite icon, float cooldown)
    //{
    //    iconImage.sprite = icon; // 스킬 아이콘 이미지 설정
    //    cooldownText.text = cooldown.ToString(); // 쿨다운 텍스트 설정
    //}

    //// 스킬을 사용하는 함수
    //public void UseSkill()
    //{
    //    if (!isCoolingDown)
    //    {
    //        // 스킬 사용 코드
    //        isCoolingDown = true;
    //        StartCoroutine(CoolDown());
    //    }
    //}

    //// 쿨다운을 처리하는 코루틴
    //private IEnumerator CoolDown()
    //{
    //    float remainingTime = float.Parse(cooldownText.text);
    //    while (remainingTime > 0)
    //    {
    //        remainingTime -= Time.deltaTime;
    //        cooldownText.text = Mathf.Round(remainingTime).ToString();
    //        yield return null;
    //    }
    //    isCoolingDown = false;
    //}
}

