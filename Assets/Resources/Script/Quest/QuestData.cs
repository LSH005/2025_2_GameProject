using UnityEngine;

[CreateAssetMenu(fileName ="New Quest", menuName ="Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string questTitle = "�� ����Ʈ"; // ����Ʈ ����

    [TextArea(2,4)]
    public string description = "����Ʈ ����";   // ����Ʈ ���� (*����)
    public Sprite questIcon;    // ����Ʈ ������

    [Header("����Ʈ ����")]
    public QuestType questType;
    public int targetAmount = 1;


    [Header("��� ����Ʈ��")]
    public Vector3 delivaryPosition;
    public float delivaryRedius = 3f;

    [Header("����/��ȣ�ۿ� ����Ʈ��")]
    public string targetTag = "";

    [Header("����")]
    public int experienceReward = 100;
    public string rewardMessage = "����Ʈ �Ϸ�";

    [Header("����Ʈ ����")]
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
                return isCompleted ? "��� �Ϸ�" : "�������� �̵��ϱ�";

            case QuestType.Collect:
            case QuestType.Interect:
                return $"{currentProgress} / {targetAmount}";

            default:
                return "";
        }
    }
}
