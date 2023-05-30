using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public List<Slot> slots;
    int workingSlotIdx = -1;
    public UnityEngine.UI.Image ClickedIcon;

    public void OnBeginDrag(PointerEventData eventData)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].RC.Contains(eventData.position))
            {
                if (slots[i].icon?.sprite)
                {
                    Sprite src = slots[i].icon.sprite;
                    workingSlotIdx = i;
                    ClickedIcon.sprite = src;
                    ClickedIcon.transform.position = eventData.position;
                    ClickedIcon.gameObject.SetActive(true);
                }
                break;
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (workingSlotIdx < 0) return;
        ClickedIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (workingSlotIdx < 0) return;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].RC.Contains(eventData.position))
            {
                Sprite src = slots[i].icon.sprite;
                slots[i].icon.sprite = ClickedIcon.sprite;
                slots[workingSlotIdx].icon.sprite = src;
                break;
            }
        }

        ClickedIcon.gameObject.SetActive(false);
        ClickedIcon.sprite = null;
        workingSlotIdx = -1;
    }
}
