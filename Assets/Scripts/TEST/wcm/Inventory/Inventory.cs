using UnityEngine;


public class Inventory : MonoBehaviour
{
    //����� �κ��丮 ����Ʈ
    public string[] itemList;
    public Slot[] slots;
    
    //�κ��丮�� ũ��
    public int numSlots;

    private void Awake()
    {
        numSlots = GameManager.Items.maxSlots;
        itemList = new string[numSlots];
        slots = GetComponentsInChildren<Slot>();
    }

    //����
    private void OnEnable()
    {
        foreach (Slot slot in slots)
        {
            slot.AddItemEvent?.AddListener(AddItem);
            slot.RemoveItemEvent?.AddListener(RemoveItem);
        }
    }

    //�̺�Ʈ����
    private void OnDisable()
    {
        foreach(Slot slot in slots)
        {
            slot.AddItemEvent?.RemoveListener(AddItem);
            slot.RemoveItemEvent?.RemoveListener(RemoveItem);
        }
    }

    public void AddItem(int slotnum, string itemName)
    {
        itemList[slotnum] = itemName;

        GameManager.Items.Add(slotnum, itemName);
    }

    public void RemoveItem(int slotnum)
    {
        itemList[slotnum] = null;

        GameManager.Items.remove(slotnum);
    }
}