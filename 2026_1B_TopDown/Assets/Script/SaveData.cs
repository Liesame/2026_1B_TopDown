[System.Serializable]
public class SaveData
{
    public int deathCount;
    public bool knifeGet;
    public bool startPlayer;
    public bool killEnemy;
    public bool nextStage;

    // 도전과제 완료 여부 저장용 (Dictionary는 직렬화가 안 되므로 배열 사용)
    public bool[] achievementCompleted = new bool[5];

    public int totalScore;
}