using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardUIControll : MonoBehaviour
{
    [SerializeField] private Button leftCard;
    [SerializeField] private Button rightCard;

    Vector2 orglcTrans;
    Vector2 orgrcTrans;

    Vector2 orglcScale;
    Vector2 orgrcScale;
    // 초기화 -1 이유 : 유효하지 않는 인덱스 값을 통해 안정성
    public int selectedCardIndex = -1;

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
        selectedCardIndex = -1;
    }

    void ScaleUpLeftCard()
    {
        Vector2 pos = leftCard.transform.position;
        leftCard.transform.SetAsLastSibling();

        leftCard.gameObject.transform.position = new Vector2(960f, pos.y);
        leftCard.gameObject.transform.localScale = new Vector3(1.4f, 1.4f);
        selectedCardIndex = 0;
        StartCoroutine(HideCardUI());
        ScaleDownRightCard();
    }

    void ScaleDownLeftCard()
    {
        leftCard.gameObject.SetActive(false);
    }

    void ScaleUpRightCard()
    {
        Vector2 pos = rightCard.transform.position;
        rightCard.transform.SetAsLastSibling();

        rightCard.gameObject.transform.position = new Vector2(960f, pos.y);
        rightCard.gameObject.transform.localScale = new Vector3(1.4f, 1.4f);
        selectedCardIndex = 1;
        StartCoroutine(HideCardUI());
        ScaleDownLeftCard();
    }

    void ScaleDownRightCard()
    {
        rightCard.gameObject.SetActive(false);
    }

    IEnumerator HideCardUI()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        Init();
        gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
