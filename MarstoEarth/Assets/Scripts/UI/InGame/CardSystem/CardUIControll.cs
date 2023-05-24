using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardUIControll : UI
{
    [SerializeField] private Button leftCard;
    [SerializeField] private Button rightCard;
    [SerializeField] private Button rerollButton;

    Vector2 orglcTrans;
    Vector2 orgrcTrans;

    Vector2 orglcScale;
    Vector2 orgrcScale;

    public CardInfo cardInfo;
    public CombatUI combatUI;

    CardHover[] cardHovers;

    private void Awake()
    {
        // CardUI의 각 위치 및 크기를 저장함 카드 호버의 IsBool 값을 위해 GetComponenet 사용
        orglcTrans = leftCard.GetComponent<RectTransform>().anchoredPosition;
        orgrcTrans = rightCard.GetComponent<RectTransform>().anchoredPosition;

        orglcScale = leftCard.gameObject.transform.localScale;
        orgrcScale = rightCard.gameObject.transform.localScale;

        cardHovers = GetComponentsInChildren<CardHover>();
    }

    void Start()
    {
        // 각 카드에 온 클릭에 대한 이벤트 반응 함수를 넣음
        leftCard.onClick.AddListener(() => ScaleUpCard(0, leftCard, rightCard, cardInfo.randomIndexLeft, rerollButton));
        rightCard.onClick.AddListener(() => ScaleUpCard(1, rightCard, leftCard, cardInfo.randomIndexRight, rerollButton));
    }

    void Init()
    {
        // 카드의 위치 및 크기를 원래 상태로 복귀 후 각 게임 오브젝트를 활성화
        leftCard.GetComponent<RectTransform>().anchoredPosition = orglcTrans;
        rightCard.GetComponent<RectTransform>().anchoredPosition = orgrcTrans;

        leftCard.gameObject.transform.localScale = orglcScale;
        rightCard.gameObject.transform.localScale = orgrcScale;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
    }

    // 카드 이벤트 반응형 함수
    void ScaleUpCard(int index, Button button, Button scaleDownButton, int skillIndex, Button rerollButton)
    {
        cardHovers[index].isBool = true;
        MoveNSize(button);
        AudioManager.Instance.PlayEffect(0);
        SkillPlus(skillIndex);
        ScaleDownCard(scaleDownButton, rerollButton);
        AudioManager.Instance.UnPauseSorce();
        UIManager.Instance.aimImage.gameObject.SetActive(true);
    }

    // 카드 위치 변동 및 크기 조절 함수
    void MoveNSize(Button button)
    {
        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector2 size = button.GetComponent<RectTransform>().sizeDelta;
        Vector2 pos = new Vector2(center.x, center.y);
        button.transform.SetAsLastSibling();
        button.gameObject.transform.position = pos;
        button.gameObject.transform.localScale = new Vector2(1.3f, 1.3f);
    }

    // 카드에 저장된 스킬을 추가 시 카드가 스킬 슬롯으로 들어가게 만들고 해당하는 스킬이 중복으로 뜨지 않도록 스킬의 리스트를 삭제함, 코루틴 함수를 발동시킴
    void SkillPlus(int skillIndex)
    {
        combatUI.LearnSkill(skillIndex);
        InGameManager.Instance.inGameSkill.RemoveAt(skillIndex);
        StartCoroutine(HideCardUI());
    }

    // 카드 이벤트 반응형 함수를 통해 크기가 커진 함수 제외한 나머지 UI 비활성화 함수
    void ScaleDownCard(Button button, Button reroll)
    {
        button.gameObject.SetActive(false);
        reroll.gameObject.SetActive(false);
    }

    // 코루틴 함수로 0.5초 이후 움직일 수 있도록 하며 Init 함수를 적용해 모든 UI를 초기화 시킴 그 후 CardHover의 isBool을 false로 하여 카드 호버 함수가 크기 조절을 못하게 함
    IEnumerator HideCardUI()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Init();
        cardHovers[0].isBool = false;
        cardHovers[1].isBool = false;
    }
}
