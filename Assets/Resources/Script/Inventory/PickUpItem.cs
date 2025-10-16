public class PickUpItem : InteractableObject
{
    public ItemData itemData;
    public int amount = 1;

    public override void Interact()
    {
        base.Interact();

        if (InventoryManager.Instance != null)
        {
            bool added = InventoryManager.Instance.AddItem(itemData, amount);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}
