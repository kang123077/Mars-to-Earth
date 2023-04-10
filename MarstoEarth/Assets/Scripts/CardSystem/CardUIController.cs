using UnityEngine;
using UnityEngine.UI;


public class CardUIController : MonoBehaviour
{
    //private Image selectedUI;
    public Button[] buttonarr;


    private void Awake()
    {
        //selectedUI = transform.GetChild(0).GetComponent<Image>();
        // buttonarr 배열 초기화
        buttonarr = new Button[2];

        // buttonarr 배열 요소에 버튼 오브젝트 할당
        buttonarr[0] = transform.GetChild(0).GetComponent<Button>();
        buttonarr[1] = transform.GetChild(1).GetComponent<Button>();

        AddListener();
    }

    public void AddListener()
    {
        buttonarr[0].onClick.AddListener(() => ScaleChageCard(buttonarr[0]));
        buttonarr[1].onClick.AddListener(() => ScaleChageCard(buttonarr[1]));
    }

    public void ScaleChageCard(Button button)
    {
        if (button == buttonarr[0])
        {
            ScaleUpLeftCard();
        }
        else if (button == buttonarr[1])
        {
            ScaleUpRightCard();
        }
    }

    private void ScaleUpLeftCard()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(200f, 0f);
        ScaleDownRightCard();
    }



    private void ScaleDownLeftCard()
    {
        buttonarr[0].gameObject.SetActive(false);
    }

    private void ScaleUpRightCard()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-200f, 0f);
        ScaleDownLeftCard();
    }

    private void ScaleDownRightCard()
    {
        buttonarr[1].gameObject.SetActive(false);
    }
}


