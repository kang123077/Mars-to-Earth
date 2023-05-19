public static class MapInfo
{
    public static int seed_Number = 0;
    public static int difficulty = 0;
    // MapManager의 초기화 이후에 1로 증가
    public static int cur_Stage = 0;
    public static int node_num = 4;
    public static NodePool cur_NodePool = NodePool.All;
    public static bool isRetry = false;
    // 노드 생성 시 시작 방과의 거리에 따른 방 생성 확률
    public static float maxDistance = 3f;
}
