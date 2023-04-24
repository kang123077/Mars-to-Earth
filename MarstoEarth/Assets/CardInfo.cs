using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CardInfo : MonoBehaviour
{
    public Image cardSkillIconLeft;
    public Image cardSkillIconRight;
    public TextMeshProUGUI cardLeftText;
    public TextMeshProUGUI cardRightText;

    void Start()
    {
        // Resources 폴더에서 스킬 정보 로드하기
        Skill.SkillInfo[] skillInfo = Resources.LoadAll<Skill.SkillInfo>("SkillsStat");
        // 랜덤한 스킬 아이콘 선택하기
        int randomIndexLeft = Random.Range(0, skillInfo.Length);
        int randomIndexRight = Random.Range(0, skillInfo.Length);
        SkillIcon(skillInfo ,randomIndexLeft, randomIndexRight);
        SkillDescription(skillInfo, randomIndexLeft, randomIndexRight);
    }

    public void SkillIcon(Skill.SkillInfo[] skillInfo, int leftIndex, int rightIndex)
    {
        Sprite randomSkillIconLeft = skillInfo[leftIndex].icon;
        Sprite randomSkillIconRight = skillInfo[rightIndex].icon;

        // 같은 아이콘이 나올 때 Left 아이콘의 인덱스를 늘려 중복을 피함
        if (randomSkillIconLeft == randomSkillIconRight)
        {
            rightIndex = (rightIndex + 1) % skillInfo.Length;
            randomSkillIconLeft = skillInfo[rightIndex].icon;
        }

        // 카드 이미지에 선택된 스킬 아이콘 할당하기
        cardSkillIconLeft.sprite = randomSkillIconLeft;
        cardSkillIconRight.sprite = randomSkillIconRight;
    }

    public void SkillDescription(Skill.SkillInfo[] skillInfo, int leftInfo, int rightInfo)
    {
        cardLeftText.text = skillInfo[leftInfo].name + "\n\n" + skillInfo[leftInfo].description;
        cardRightText.text = skillInfo[rightInfo].name + "\n\n" + skillInfo[rightInfo].description;
    }

    void Update()
    {

    }
}
