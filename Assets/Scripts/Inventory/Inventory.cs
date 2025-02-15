using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    DragAndDropItem DragAndDropItem;
    InventorySlot InventorySlot;

    [SerializeField] private List<InventorySlot> InventoryList;
    [SerializeField] private Transform slotsParent;

    GameObject inventory;

    public static Inventory instance;
    public bool isOpened;
    public bool mouseMoveDrag;   // Происходит ли перетаскивание объекта (во избежании конфликтов с другими действиями)

    private void Awake()
    {
        InventorySlot = FindObjectOfType<InventorySlot>();
        InventoryList = new List<InventorySlot>();
        DragAndDropItem = FindObjectOfType<DragAndDropItem>();
    }

    private void Start()
    {
        StartInventoryFilling();
    }

    private void StartInventoryFilling()
    {
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            Transform child = slotsParent.GetChild(i);
            InventorySlot slot = child.GetComponent<InventorySlot>();
            InventoryList.Add(slot);
        }
    }

    public void PutInEmptySlot(Item item)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i].IsEmpty())
            {
                InventoryList[i].PutInSlot(item);
                InventoryList[i].GetComponent<InventorySlot>().QuantityText();
                return;
            }
        }
    }

    // Метод для получения списка слотов
    public List<InventorySlot> GetSlotList()
    {
        return InventoryList;
    }

    public int GetSlotIndex(InventorySlot slot)
    {
        return InventoryList.IndexOf(slot);
    }

    public void SwapItems(List<InventorySlot> itemList, int indexA, int indexB)
    {
        // Меняем местами элементы
        InventorySlot temp = itemList[indexA];
        itemList[indexA] = itemList[indexB];
        itemList[indexB] = temp;
    }

    public void OpenClose()
    {
        inventory.SetActive(isOpened);
        isOpened = !isOpened;
    }
}
