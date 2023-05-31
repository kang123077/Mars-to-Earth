using UnityEngine;

public class TestDungeonInfo : MonoBehaviour
{
    public DungeonName dungeonName;
    public StageInfo[] stageInfo;
    public int curStage;
}
public enum DungeonName
{
    Mars,
    Moon,
    Earth
}