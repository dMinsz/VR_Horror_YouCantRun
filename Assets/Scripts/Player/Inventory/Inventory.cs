using UnityEngine;


public class Inventory : MonoBehaviour
{
    //저장될 인벤토리 리스트
    public string[] itemList;
    public Slot[] slots;
    
    //인벤토리의 크기
    public int numSlots;

    private void Awake()
    {
        numSlots = GameManager.Items.maxSlots;
        itemList = new string[numSlots];
        slots = GetComponentsInChildren<Slot>();
    }

    //구독
    private void OnEnable()
    {
        foreach (Slot slot in slots)
        {
            slot.AddItemEvent?.AddListener(AddItem);
            slot.RemoveItemEvent?.AddListener(RemoveItem);
        }
    }

    //이벤트제거
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