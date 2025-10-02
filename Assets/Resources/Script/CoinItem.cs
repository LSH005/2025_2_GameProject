using Unity.VisualScripting;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("동전 설정")]
    public int coinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "동전";
        interactionText = "[E] 동전 쌔비기";
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
