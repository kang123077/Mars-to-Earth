using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    private RawImage rawImage;
    private Vector2 screenSize;
    public Vector2 size;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        screenSize = new Vector2(Screen.width, Screen.height);
        rawImage.rectTransform.sizeDelta = new Vector2(screenSize.x / 16f * size.x, screenSize.y / 9f * size.y);
    }

    private void Update()
    {
        // 스크린 사이즈가 변경되었는지 체크합니다.
        if (screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            screenSize = new Vector2(Screen.width, Screen.height);
            rawImage.rectTransform.sizeDelta = new Vector2(screenSize.x / 16f * size.x, screenSize.y / 9f * size.y);
        }
    }
}
