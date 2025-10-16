using UnityEngine;

public class QuestGiverNPC : InteractableObject
{

    [Header("NPC Quest Settings")]
    public QuestData questToGive;
    public string npcName = "NPC";
    public string questStartMessege = "새로운 퀘스트가 있습니다.";
    public string noQuestMessege = "퀘스트가 없습니다.";
    public string questAlreadyActiveMessege = "이미 진행중인 퀘스트가 있습니다.";

    private QuestManager questManager;

    protected override void Start()
    {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();

        if (questManager == null)
        {
            Debug.LogError("QuestManager 없음");

            interactionText = "[E]" + npcName + "와 대화하기";
        }

    }

    public override void Interact()
    {
        base.Interact();
        questManager.StartQuest(questToGive);
    }
}
