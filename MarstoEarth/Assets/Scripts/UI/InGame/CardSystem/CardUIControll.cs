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
        AudioManager.Instance.PlayEffect(0);
        int skillIndex = cardInfo.randomIndexLeft;
        SkillPlus(skillIndex);
        ScaleDownCard(rightCard, rerollButton);
        AudioManager.Instance.UnPauseSorce();
        UIManager.Instance.aimImage.gameObject.SetActive(true);
    }

    void ScaleUpRightCard()
    {
        cardHovers[1].isBool = true;
        MoveNSize(rightCard);
        AudioManager.Instance.PlayEffect(0);
        int skillIndex = cardInfo.randomIndexRight;
        SkillPlus(skillIndex);
        ScaleDownCard(leftCard, rerollButton);
        AudioManager.Instance.UnPauseSorce();
        UIManager.Instance.aimImage.gameObject.SetActive(true);
    }

    void MoveNSize(Button button)
    {
        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector2 size = button.GetComponent<RectTransform>().sizeDelta;
        Vector2 pos = new Vector2(center.x, center.y);
        button.transform.SetAsLastSibling();
        button.gameObject.transform.position = pos;
        button.gameObject.transform.localScale = new Vector2(1.3f, 1.3f);
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
