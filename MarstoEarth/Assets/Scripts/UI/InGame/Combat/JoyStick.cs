
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour ,IPointerDownHandler,   IDragHandler, IEndDragHandler 
{
    
    public Image innerStick;
    private Vector3 startPos;

    public static Vector3 dir;

    private float radius;
    private Vector3 curPos;
    
    public RectTransform rcTr;
    
    void Start()
    {
        

    }
    private void Awake()
    {
        startPos = transform.position;
        radius = rcTr.sizeDelta.y * 0.5f;
        dir = Vector3.zero;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
       
        Vector3 pos = eventData.position;
        float distance = Vector3.Distance(startPos, pos);
        dir = (pos - startPos).normalized;
        if(distance < radius)
        {
            
            innerStick.transform.position = pos;
        }
        else
        {
            
            distance = radius;
            innerStick.transform.position = startPos + dir * radius;
        }
        
        SpawnManager.Instance.player.xInput= dir.x*distance* (1/radius);
        SpawnManager.Instance.player.zInput= dir.y*distance* (1/radius);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        float distance = Vector3.Distance(startPos, pos);
        dir = (pos - startPos).normalized;
        if(distance < radius)
        {
            
            innerStick.transform.position = pos;
        }
        else
        {
            
            distance = radius;
            innerStick.transform.position = startPos + dir * radius;
        }
        
        SpawnManager.Instance.player.xInput= dir.x*distance* (1/radius);
        SpawnManager.Instance.player.zInput= dir.y*distance* (1/radius);
        Debug.Log(SpawnManager.Instance.player.xInput);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        innerStick.transform.position = startPos;
    }
}