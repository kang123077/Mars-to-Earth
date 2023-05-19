public static class MapInfo
{
    public static int seed_Number = 0;
    public static int difficulty = 0;
    // MapManager의 초기화 이후에 1로 증가
    public static int cur_Stage = 0;
    public static int node_num = 1;
    public static NodePool cur_NodePool = NodePool.All;
    public static bool isRetry = false;
}
