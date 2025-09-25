using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("��ȣ �ۿ� ����")]
    public string objectName = "������";
    public string interactionText = "[E] ��ȣ �ۿ�";
    public InteractionType interactionType = InteractionType.Item;

    [Header("���̶���Ʈ ����")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    public Renderer objectRenderer;
    private Color originalColor;

    private bool isHighlighted = false;

    public enum InteractionType
    {
        Item,       //������ (����, ���� ��)
        Machine,    //���� (����, ��ư ��)
        Building,   //�ǹ� (��, ���� ��)
        NPC,
        Collectible //����ǰ
    }

    public virtual void Interact()
    {
        //��ȣ �ۿ� Ÿ�Կ� ���� �⺻ ����
        switch (interactionType)
        {
            case InteractionType.Item:
                CollectItem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Building:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break;
            case InteractionType.NPC:
                TalkToNPC();
                break;
        }
    }

    protected virtual void CollectItem()
    {
        //������ ���� �����Լ�
        Destroy(gameObject); //�������� �ı��Ѵ�.
    }

    protected virtual void OperateMachine()
    {
        //��� �۵� �Լ�
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.green; //�켱 ���� �ʷϻ����� �����.
        }
    }

    protected virtual void AccessBuilding()
    {
        //���� ����
        transform.Rotate(Vector3.up * 90f); //�켱 ȸ���Ѵ�.
    }

    protected virtual void TalkToNPC()
    {
        //NPC�� ��ȭ
        Debug.Log($"{objectName}�� ��ȭ�� �����մϴ�."); //�켱 ����� �α׸� �Ѵ�.
    }

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
        gameObject.layer = 8; //(Layer 8 = Interactable) �������� �Ѵ�.
    }

    public virtual void OnPlayerEnter()
    {
        Debug.Log($"[{objectName}] ������");
        HighlightObject();
    }

    public virtual void OnPlayerExit()
    {
        Debug.Log($"[{objectName}] �������� ���");
        RemoveHighlight();
    }

    public string GetInteractionText()
    {
        //UI�� ������ Text ���� �Լ�
        return interactionText;
    }

    protected virtual void HighlightObject()
    {
        //���� �Լ��� ���̶���Ʈ ����
        if (objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", highlightIntensity);
            isHighlighted = true;
        }
    }

    protected virtual void RemoveHighlight()
    {
        //���� �Լ��� ���̶���Ʈ ���� ����
        if (objectRenderer != null && isHighlighted)
        {
            objectRenderer.material.color = originalColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
    }
}

