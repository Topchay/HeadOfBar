using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Inventory Inventory;
    public Item originalItemObj;
    public Item itemObj;

    private void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
        itemObj = Instantiate(originalItemObj);
    }

    private void OnMouseDown()
    {
        if (!FindObjectOfType<DragAndDropItem>().CheckUI())
        {
            Inventory.PutInEmptySlot(itemObj);
            Destroy(gameObject);
        }
    }
}
