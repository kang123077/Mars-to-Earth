using UnityEngine;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour// ,IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public Image innerStick;
    private static Vector2 startPos;

    public static Vector2 dir;

    private static float radius;

    private void Awake()
    {

        radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        dir = Vector2.zero;
    }
    private void OnEnable()
    {
        startPos = transform.position;
    }


    public void OnDrag(Vector2 curPos)
    {

        float distance = Vector2.Distance(startPos, curPos);
        dir = (curPos - startPos).normalized;
        if (distance < radius)
        {

            innerStick.transform.position = curPos;
        }
        else
        {

            distance = radius;
            innerStick.transform.position = startPos + dir * radius;
        }

        SpawnManager.Instance.player.xInput = dir.x * distance * (1 / radius);
        SpawnManager.Instance.player.zInput = dir.y * distance * (1 / radius);
    }



}