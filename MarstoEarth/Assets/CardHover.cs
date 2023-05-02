using UnityEngine.EventSystems;
using UnityEngine;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform leftCardTransform;
    private RectTransform rightCardTransform;

    private void Awake()
    {
        leftCardTransform = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        rightCardTransform = transform.GetChild(1).gameObject.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered CardUI");

        if (eventData.pointerCurrentRaycast.gameObject.transform == leftCardTransform)
        {
            Debug.Log("Enter if 문은 돌고있다");
            leftCardTransform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform == rightCardTransform)
        {
            Debug.Log("Enter else if 문은 돌고있다");
            rightCardTransform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited CardUI");

        if (eventData.pointerCurrentRaycast.gameObject.transform == leftCardTransform)
        {
            Debug.Log("Exit if 문은 돌고있다");
            leftCardTransform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform == rightCardTransform)
        {
            Debug.Log("Exit else if 문은 돌고있다");
            rightCardTransform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}

