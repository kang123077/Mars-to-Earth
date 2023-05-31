using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHovers : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = Vector3.one;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle()
    }
}