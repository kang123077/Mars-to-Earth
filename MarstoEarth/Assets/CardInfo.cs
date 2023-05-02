using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class CardInfo : MonoBehaviour
{
    public Image cardSkillIconLeft;
    public Image cardSkillIconRight;
    public TextMeshProUGUI cardLeftText;
    public TextMeshProUGUI cardRightText;
    [HideInInspector]public int randomIndexLeft;
    [HideInInspector]public int randomIndexRight;

    public void CardInit()
    {
        // 랜덤한 스킬 아이콘 선택하기
        randomIndexLeft = Random.Range(0, InGameManager.Instance.inGameSkillInfo.Count);
        randomIndexRight = Random.Range(0, InGameManager.Instance.inGameSkillInfo.Count);
        SkillIcon(randomIndexLeft, randomIndexRight);
        SkillDescription(randomIndexLeft, randomIndexRight);
    }

    public void CardReroll()
    {
        // 랜덤한 스킬 아이콘 선택하기
        randomIndexLeft = Random.Range(0, InGameManager.Instance.inGameSkillInfo.Count);
        randomIndexRight = Random.Range(0, InGameManager.Instance.inGameSkillInfo.Count);
        SkillIcon(randomIndexLeft, randomIndexRight);
        SkillDescription(randomIndexLeft, randomIndexRight);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void SkillIcon(int leftIndex, int rightIndex)
    {
        Sprite randomSkillIconLeft = InGameManager.Instance.inGameSkillInfo[leftIndex].icon;
        Sprite randomSkillIconRight = InGameManager.Instance.inGameSkillInfo[rightIndex].icon;

        // 같은 아이콘이 나올 때 Left 아이콘의 인덱스를 늘려 중복을 피함
        if (randomSkillIconLeft == randomSkillIconRight)
        {
            rightIndex = (rightIndex + 1) % InGameManager.Instance.inGameSkillInfo.Count;
            randomSkillIconLeft = InGameManager.Instance.inGameSkillInfo[rightIndex].icon;
        }

        // 카드 이미지에 선택된 스킬 아이콘 할당하기
        cardSkillIconLeft.sprite = randomSkillIconLeft;
        cardSkillIconRight.sprite = randomSkillIconRight;
    }

    public void SkillDescription(int leftInfo, int rightInfo)
    {
        cardLeftText.text = InGameManager.Instance.inGameSkillInfo[leftInfo].name + "\n\n" + InGameManager.Instance.inGameSkillInfo[leftInfo].description;
        cardRightText.text = InGameManager.Instance.inGameSkillInfo[rightInfo].name + "\n\n" + InGameManager.Instance.inGameSkillInfo[rightInfo].description;
    }

    void Update()
    {
        
    }
}
