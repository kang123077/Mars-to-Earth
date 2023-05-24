using UnityEngine;

public class GameExplainUI : MonoBehaviour
{
    public Sprite[] spriteArray; // 이미지 배열 선언
    public UnityEngine.UI.Image explainImage;
    private int currentImageIndex = 0; // 현재 이미지 인덱스를 저장할 변수
    void Start()
    {

    }

    public void ChangeImage()
    {
        if (spriteArray.Length > 0) // 이미지 배열에 이미지가 있는지 확인합니다.
        {
            currentImageIndex++; // 다음 이미지 인덱스로 이동합니다.

            if (currentImageIndex >= spriteArray.Length) // 이미지 인덱스가 배열 범위를 벗어날 경우
            {
                currentImageIndex = 0; // 첫 번째 이미지로 돌아갑니다.
            }

            explainImage.sprite = spriteArray[currentImageIndex]; // 이미지를 변경합니다.
        }
    }

    public void GameExplain()
    {
        gameObject.SetActive(true);
        explainImage.sprite = spriteArray[0];
    }
}
