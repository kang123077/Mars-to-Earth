using UnityEngine;

public class RoomClearEvent : MonoBehaviour
{
    public GameObject[] cardUIs; // 인스펙터 창에서 직접 할당할 수 있도록 변경
    private int clearedRooms = 0; // 클리어한 방의 개수를 저장하는 변수

    /*
     * 맵을 클리어할 때 이벤트를 발생시키고
     * 맵의 특정한 구역에 적용하면 구역 통과시
     * 맵 클리어할 때 발동되는 코드
     */
    public void OnRoomCleared()
    {
        clearedRooms++;
        if (clearedRooms >= 2) // 두 개의 방을 클리어했을 때
        {
            TriggerEvent();
        }
    }

    private void TriggerEvent()
    {
        foreach (GameObject cardUI in cardUIs)
        {
            if (cardUI != null && !cardUI.activeSelf)
            {
                cardUI.SetActive(true);
            }
        }
    }
}
