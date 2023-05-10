using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class CardInfo : MonoBehaviour
{
    public Image cardSkillIconLeft;
    public Image cardSkillIconRight;
    public GameObject reroll;
    public TextMeshProUGUI cardLeftText;
    public TextMeshProUGUI cardRightText;
    [HideInInspector]public int randomIndexLeft;
    [HideInInspector]public int randomIndexRight;

    public void CardInit()
    {
        // 랜덤한 스킬 아이콘 선택하기
        randomIndexLeft = Random.Range(0, InGameManager.Instance.inGameSkill.Count);
        randomIndexRight = Random.Range(0, InGameManager.Instance.inGameSkill.Count);
        SkillIcon(randomIndexLeft, randomIndexRight);
        SkillDescription(randomIndexLeft, randomIndexRight);
    }

    public void CardReroll()
    {
        // 랜덤한 스킬 아이콘 선택하기
        randomIndexLeft = Random.Range(0, InGameManager.Instance.inGameSkill.Count);
        randomIndexRight = Random.Range(0, InGameManager.Instance.inGameSkill.Count);
        SkillIcon(randomIndexLeft, randomIndexRight);
        SkillDescription(randomIndexLeft, randomIndexRight);
        AudioManager.Instance.PlayEffect(1);
        reroll.SetActive(false);
    }

    public void SkillIcon(int leftIndex, int rightIndex)
    {
        Sprite randomSkillIconLeft = InGameManager.Instance.inGameSkill[leftIndex].skillInfo.icon;
        Sprite randomSkillIconRight = InGameManager.Instance.inGameSkill[rightIndex].skillInfo.icon;

        // 같은 아이콘이 나올 때 Left 아이콘의 인덱스를 늘려 중복을 피함
        if (randomSkillIconLeft == randomSkillIconRight)
        {
            rightIndex = (rightIndex + 1) % InGameManager.Instance.inGameSkill.Count;
            randomSkillIconLeft = InGameManager.Instance.inGameSkill[rightIndex].skillInfo.icon;
        }

        // 카드 이미지에 선택된 스킬 아이콘 할당하기
        cardSkillIconLeft.sprite = randomSkillIconLeft;
        cardSkillIconRight.sprite = randomSkillIconRight;
    }

    public void SkillDescription(int leftInfo, int rightInfo)
    {
        cardLeftText.text = InGameManager.Instance.inGameSkill[leftInfo].skillInfo.name + "\n\n" + InGameManager.Instance.inGameSkill[leftInfo].skillInfo.description;
        cardRightText.text = InGameManager.Instance.inGameSkill[rightInfo].skillInfo.name + "\n\n" + InGameManager.Instance.inGameSkill[rightInfo].skillInfo.description;
    }
}
