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

    private void Awake()
    {
        orglcTrans = leftCard.gameObject.transform.position;
        orgrcTrans = rightCard.gameObject.transform.position;

        orglcScale = leftCard.gameObject.transform.localScale;
        orgrcScale = rightCard.gameObject.transform.localScale;
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
        MoveNSize(leftCard);
        int skillIndex = cardInfo.randomIndexLeft;
        SkillPlus(skillIndex);
        ScaleDownRightCard();
    }

    void ScaleDownLeftCard()
    {
        leftCard.gameObject.SetActive(false);
        rerollButton.gameObject.SetActive(false);
    }

    void ScaleUpRightCard()
    {
        MoveNSize(rightCard);
        int skillIndex = cardInfo.randomIndexRight;
        SkillPlus(skillIndex);
        ScaleDownLeftCard();
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
        InGameManager.Instance.inGameSkillInfo.RemoveAt(skillIndex);
        StartCoroutine(HideCardUI());
    }

    void ScaleDownRightCard()
    {
        rightCard.gameObject.SetActive(false);
        rerollButton.gameObject.SetActive(false);
    }

    IEnumerator HideCardUI()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        Init();
        gameObject.SetActive(false);
    }
}
