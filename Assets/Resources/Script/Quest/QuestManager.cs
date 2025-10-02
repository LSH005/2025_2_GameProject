using UnityEngine;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI 요소들")]
    public GameObject questUI;
    public Text questTitleText;
    public Text questDescriptionText;
    public Text questPrgressText;
    public Button completeButton;

    [Header("UI 요소들")]
    public QuestData[] availableQuests;

    private QuestData currentQuest;
    private int currentQuestIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);
        }
        if (completeButton != null)
        {
            completeButton.onClick.AddListener(CompleteCurrentQuest);
        }
    }


    void Update()
    {
        if (currentQuest != null && currentQuest.isActive)
        {
            CheckQuestProgress();
            UpdateQuestUI();
        }
    }

    void UpdateQuestUI()
    {
        if (currentQuest == null) return;

        if (questTitleText != null)
        {
            questTitleText.text = currentQuest.questTitle;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.description;
        }

        if (questPrgressText != null)
        {
            questPrgressText.text = currentQuest.GetProjressText();
        }
    }

    public void StartQuest(QuestData quest)
    {
        if (quest == null) return;

        currentQuest = quest;
        currentQuest.Initalize();
        currentQuest.isActive = true;

        Debug.Log("퀘스트 시작 : " + questTitleText);
        UpdateQuestUI();
        if (questUI != null)
        {
            questUI.SetActive(true);
        }

    }

    void CheckDelivaryProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.delivaryPosition);
        if (distance <= currentQuest.delivaryRedius)
        {
            if (currentQuest.currentProgress == 0)
            {
                currentQuest.currentProgress = 1;
            }
        }
        else
        {
            currentQuest.currentProgress = 0;
        }
    }

    public void AddIndteractProgress(string objectTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Interect && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("상호작용됨 : " + objectTag);
        }
    }

    public void CompleteCurrentQuest()
    {
        if (currentQuest == null || !currentQuest.isCompleted) return;

        Debug.Log("퀘스트 완료 : " + currentQuest.rewardMessage);
        if (completeButton != null)
        {
            completeButton.gameObject.SetActive(false);
        }

        currentQuestIndex++;
        if (currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
            currentQuest = null;
            if (questUI != null)
            {
                questUI.gameObject.SetActive(false);
            }
        }
    }

    void CheckQuestProgress()
    {
        if (currentQuest.questType == QuestType.Delivery)
        {
            CheckDelivaryProgress();
        }

        if (currentQuest.IsComplete() && !currentQuest.isCompleted)
        {
            completeButton.gameObject.SetActive(true);
        }
    }
}
