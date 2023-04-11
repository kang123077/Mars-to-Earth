using System.Collections;
using System.Collections.Generic;
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
    }

    void ScaleUpLeftCard()
    {
        Vector2 pos = leftCard.transform.position;
        leftCard.transform.SetAsLastSibling();

        leftCard.gameObject.transform.position = new Vector2(400f, pos.y);
        leftCard.gameObject.transform.localScale = new Vector3(1.5f, 1.5f);
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
        leftCard.transform.SetAsLastSibling();

        rightCard.gameObject.transform.position = new Vector2(400f, pos.y);
        rightCard.gameObject.transform.localScale = new Vector3(1.5f, 1.5f);
        StartCoroutine(HideCardUI());
        ScaleDownLeftCard();
    }

    void ScaleDownRightCard()
    {
        rightCard.gameObject.SetActive(false);
    }

    IEnumerator HideCardUI()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        leftCard.gameObject.SetActive(false);
        rightCard.gameObject.SetActive(false);
        InGameManager.Instance.skillUICon.SetActive(true);
        Init();
    }



    void Update()
    {
        
    }
}
