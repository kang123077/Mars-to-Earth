using UnityEngine;

public class RoomClearEvent : MonoBehaviour
{
    public GameObject cardUI;  // 카드 UI 프리팹을 미리 할당
    private int clearedRooms = 0;  // 클리어한 방의 개수를 저장하는 변수

    /*
     * 맵을 클리어할 때 이벤트를 발생시키고
     * 맵의 특정한 구역에 적용하면 구역 통과시
     * 맵 클리어할 때 발동되는 코드
     */
    public void OnRoomCleared()
    {
        clearedRooms++;
        if (clearedRooms >= 2)  // 두 개의 방을 클리어했을 때
        {
            ShowCardUI();
            gameObject.SetActive(false);  // 이벤트 발생 후 삭제하는 경우 사용
        }
    }

    private void ShowCardUI()
    {
        if (cardUI != null && !cardUI.activeSelf)
        {
            cardUI.SetActive(true);
        }
    }
}