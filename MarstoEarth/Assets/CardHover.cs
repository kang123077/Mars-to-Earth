using UnityEngine.EventSystems;
using UnityEngine;

public class CardHover : MonoBehaviour, IPointerMoveHandler
{
    public RectTransform lcRT;
    public RectTransform rcRT;
    private Rect leftCard;
    private Rect rightCard;
    public Rect LC
    {
        get 
        {
            leftCard.x = lcRT.position.x - lcRT.rect.width * 0.5f;
            leftCard.y = lcRT.position.y + lcRT.rect.height * 0.5f;
            return leftCard;
        }
    }
    public Rect RC
    {
        get
        {
            rightCard.x = rcRT.position.x - rcRT.rect.width * 0.5f;
            rightCard.y = rcRT.position.y + rcRT.rect.height * 0.5f;
            return rightCard;
        }
    }


    private void Awake()
    {
        Init(LC, lcRT);
        Init(RC, rcRT);
    }

    private void Init(Rect rect, RectTransform cardRT)
    {
        rect.x = cardRT.position.x - cardRT.rect.width * 0.5f;
        rect.y = cardRT.position.y + cardRT.rect.height * 0.5f;
        rect.width = cardRT.rect.width;
        rect.height = cardRT.rect.height;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(lcRT.rect);
        Debug.Log(LC);
        ScaleFunc(LC, lcRT, eventData);
        ScaleFunc(RC, rcRT, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    private void ScaleFunc(Rect RC, RectTransform rcTR, PointerEventData eventData)
    {
        if (RC.Contains(eventData.position))
        {
            rcTR.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            rcTR.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}

