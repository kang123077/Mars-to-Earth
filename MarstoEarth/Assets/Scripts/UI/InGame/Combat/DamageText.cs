using UnityEngine;

public class DamageText : MonoBehaviour
{
    private static float moveSpeed;
    private static float alphaSpeed;
    public TextMesh text;
    //public TextMeshPro text;
    public RectTransform thisRect;
    Color alpha;

    private float lifeTime;
    private static float duration;

    private static CombatUI combatUI;
    private static Transform follower;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2f;
        alphaSpeed = .7f;
        duration = 1.2f;
        alpha = text.color;

        combatUI = (CombatUI)UIManager.Instance.UIs[(int)UIType.Combat];
        follower = CinemachineManager.Instance.follower;
    }

    private void OnEnable()
    {
        lifeTime = duration;
        alpha.a = 1;
        text.color = alpha;
        text.transform.localScale = Vector3.one * (120.0f / Screen.width);

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        transform.rotation = follower.rotation;
        if (lifeTime < 0)
            combatUI.DMGTextPool.Release(this);
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치
        alpha.a -= Time.deltaTime * alphaSpeed; // 텍스트 알파값
        text.color = alpha;
    }


}