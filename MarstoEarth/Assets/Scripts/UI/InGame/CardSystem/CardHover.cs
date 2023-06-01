using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isBool;
    void Awake()
    {
        isBool = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isBool)
        {
            gameObject.transform.localScale = new Vector3(1.1f, 1.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isBool)
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }
}


