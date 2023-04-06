using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    //public CardData cardData; // 카드 데이터
    //public SkillSlot skillSlot; // 스킬 슬롯

    private Button button; // 버튼 컴포넌트

    private void Awake()
    {
        if (TryGetComponent(out button))
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        //skillSlot.AddSkill(cardData.skill); // 카드 데이터의 스킬을 스킬 슬롯에 추가
        gameObject.SetActive(false); // 카드 UI를 비활성화
    }
}

