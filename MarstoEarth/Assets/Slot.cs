using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.UI.Image icon;

    public RectTransform rcTr;
    private Rect rc;// �»�� ���� ��Ʈ

    public Rect RC
    {
        get {
            rc.x = rcTr.position.x - rcTr.rect.width * 0.5f;
            rc.y = rcTr.position.y - rcTr.rect.height * 0.5f;
            return rc; }

    }

    
    void Start()
    {
        rc.width = rcTr.rect.width;
        rc.height = rcTr.rect.height;
    }

}
