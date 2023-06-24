using UnityEngine;

public class EncyclopediaUI : MonoBehaviour
{
    public LoadExplainSprite[] buttonsArray;
    private int currentStoryIndex = 0; // 현재 스토리 인덱스

    private void Awake()
    {

    }

    public void ChangeStoryPrev()
    {

    }

    public void ChangeStoryNext()
    {

    }

    public void EncyclopediaUIOn()
    {
        OutGameAudio.Instance.PlayEffect(1);
        gameObject.SetActive(true);
    }

    public void StoryOn(int index)
    {

    }

    public void ExitUI()
    {
        gameObject.SetActive(false);
    }

    public void UndoUI()
    {
    }
}
