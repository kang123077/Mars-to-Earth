using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI :MonoBehaviour
{
    private bool isShown= false;
    public void Show()
    {
        if(!isShown) gameObject.SetActive(isShown=true);
    }
    public void Close()
    {
        if(isShown) gameObject.SetActive(isShown=false);
    }
 
}
