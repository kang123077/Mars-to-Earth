using UnityEngine;

public class LightController : MonoBehaviour
{
    private int originalCullingMask;
    public Light lightComponent;

    private void Awake()
    {
        originalCullingMask = lightComponent.cullingMask;
    }

    public void addMonsterLayer()
    {
        lightComponent.cullingMask |= 1 << LayerMask.NameToLayer("Monster");
    }

    public void removeMonsterLayer()
    {
        lightComponent.cullingMask =
            lightComponent.cullingMask & ~(1 << LayerMask.NameToLayer("Monster"));
    }
}
