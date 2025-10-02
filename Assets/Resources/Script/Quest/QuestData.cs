using UnityEngine;

[CreateAssetMenu(fileName ="New Quest", menuName ="Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("기본 정보")]
    public string questTitle = "새 퀘스트"; // 퀘스트 제목

    [TextArea(2,4)]
    public string description = "퀘스트 설명";   // 퀘스트 설명 (*내용)
    public Sprite questIcon;    // 퀘스트 아이콘

    [Header("퀘스트 설정")]
    public QuestType questType;
    public int targetAmount = 1;


    [Header("배달 퀘스트용")]
    public Vector3 delivaryPosition;
    public float delivaryRedius = 3f;

    [Header("수집/상호작용 퀘스트용")]
    public string targetTag = "";

    [Header("보상")]
    public int experienceReward = 100;
    public string rewardMessage = "퀘스트 완료";

    [Header("퀘스트 연결")]
    public QuestData nextQuest;

    [System.NonSerialized] public int currentProgress = 0;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public bool isCompleted = false;

    public void Initalize()
    {
        currentProgress = 0;
        isActive = false;
        isCompleted = false;
    }

    public bool IsComplete()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return currentProgress >= 1;

            case QuestType.Collect:
            case QuestType.Interect:
                return currentProgress >= targetAmount;

            default:
                return false;
        }
    }

    public float GetProgressPercentage()
    {
        if (targetAmount <= 0) return 0;
        return Mathf.Clamp01((float)currentProgress / targetAmount);
    }

    public string GetProjressText()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return isCompleted ? "배달 완료" : "목적지로 이동하기";

            case QuestType.Collect:
            case QuestType.Interect:
                return $"{currentProgress} / {targetAmount}";

            default:
                return "";
        }
    }
}
