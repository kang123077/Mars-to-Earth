using UnityEngine.EventSystems;
using UnityEngine;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isBool;
    void Awake()
    {
        isBool = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isBool)
        {
            gameObject.transform.localScale = new Vector3(1.3f, 1.3f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isBool)
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }
}


