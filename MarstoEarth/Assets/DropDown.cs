using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{

    public TMPro.TMP_Dropdown uiDropDown;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log (uiDropDown.options[0].text);
        uiDropDown.value = 3;
    }

    public void OnValueChanged(int i)
    {
        Debug.Log(i);
        Debug.Log(uiDropDown.options[i].text);
    }

}
