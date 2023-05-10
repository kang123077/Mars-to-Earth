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
        orglcTrans = leftCard.gameObject.transform.position;
        orgrcTrans = rightCard.gameObject.transform.position;

        orglcScale = leftCard.gameObject.transform.localScale;
        orgrcScale = rightCard.gameObject.transform.localScale;

        cardHovers = GetComponentsInChildren<CardHover>();
    }

    void Start()
    {
        leftCard.onClick.AddListener(ScaleUpLeftCard);
        rightCard.onClick.AddListener(ScaleUpRightCard);
    }

    void Init()
    {
        leftCard.gameObject.transform.position = orglcTrans;
        rightCard.gameObject.transform.position = orgrcTrans;

        leftCard.gameObject.transform.localScale = orglcScale;
        rightCard.gameObject.transform.localScale = orgrcScale;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    void ScaleUpLeftCard()
    {
        cardHovers[0].isBool = true;
        MoveNSize(leftCard);
        AudioManager.Instance.PlayEffect(3);
        int skillIndex = cardInfo.randomIndexLeft;
        SkillPlus(skillIndex);
        ScaleDownCard(rightCard, rerollButton);
    }

    void ScaleUpRightCard()
    {
        cardHovers[1].isBool = true;
        MoveNSize(rightCard);
        AudioManager.Instance.PlayEffect(3);
        int skillIndex = cardInfo.randomIndexRight;
        SkillPlus(skillIndex);
        ScaleDownCard(leftCard, rerollButton);
    }

    void MoveNSize(Button button)
    {
        Vector2 pos = button.gameObject.transform.position;
        button.transform.SetAsLastSibling();
        button.gameObject.transform.position = new Vector3(960f, pos.y);
        button.gameObject.transform.localScale = new Vector2(1.4f, 1.4f);
    }

    void SkillPlus(int skillIndex)
    {
        combatUI.LearnSkill(skillIndex);
        InGameManager.Instance.inGameSkill.RemoveAt(skillIndex);
        Debug.Log(InGameManager.Instance.inGameSkill.Count);
        StartCoroutine(HideCardUI());
    }

    void ScaleDownCard(Button button, Button reroll)
    {
        button.gameObject.SetActive(false);
        reroll.gameObject.SetActive(false);
    }

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
