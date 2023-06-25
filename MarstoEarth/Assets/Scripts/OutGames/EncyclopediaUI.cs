using UnityEngine;

public class EncyclopediaUI : MonoBehaviour
{
    private int currentStoryIndex; // 현재 스토리 인덱스 -1일시 안열린상태
    [SerializeField]
    private GameObject arrowImage;
    [SerializeField]
    private GameObject buttons;
    [SerializeField]
    private GameObject unOpened;
    [SerializeField]
    private GameObject[] contents;

    private void Awake()
    {
        currentStoryIndex = -1;
        for (int i = MapInfo.storyValue; i < contents.Length; i++)
        {
            contents[i] = unOpened;
        }
    }

    public void ChangeStoryPrev()
    {
        contents[currentStoryIndex].SetActive(false);
        if (--currentStoryIndex < 0)
        {
            currentStoryIndex = contents.Length - 1;
        }
        contents[currentStoryIndex].SetActive(true);
    }

    public void ChangeStoryNext()
    {
        contents[currentStoryIndex].SetActive(false);
        if (++currentStoryIndex > contents.Length - 1)
        {
            currentStoryIndex = 0;
        }
        contents[currentStoryIndex].SetActive(true);
    }

    public void EncyclopediaUIOn()
    {
        OutGameAudio.Instance.PlayEffect(1);
        gameObject.SetActive(true);
    }

    public void StoryOn(int index)
    {
        currentStoryIndex = index;
        buttons.SetActive(false);
        arrowImage.SetActive(true);
        contents[index].SetActive(true);
    }

    public void ExitUI()
    {
        if (currentStoryIndex >= 0)
            contents[currentStoryIndex].SetActive(false);
        currentStoryIndex = -1;
        buttons.SetActive(true);
        gameObject.SetActive(false);
    }

    public void UndoUI()
    {
        contents[currentStoryIndex].SetActive(false);
        currentStoryIndex = -1;
        buttons.SetActive(true);
    }
}
