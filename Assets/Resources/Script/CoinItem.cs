using Unity.VisualScripting;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("���� ����")]
    public int coinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "����";
        interactionText = "[E] ���� �غ��";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {
        base.CollectItem();
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.AddIndteractProgress(questTag);
        }
        Destroy(gameObject);
    }
}
