using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;             
    public Item SlotItem;          
    public Sprite spriteEmpty;     

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    public void PutInSlot(Item item)
    {
        icon.sprite = item.icon;
        SlotItem = item;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);       
    }

    public bool IsEmpty()
    {
        return SlotItem == null;
    }

    public void QuantityText()
    {
        Transform childText = gameObject.transform.GetChild(0);

        if  (SlotItem != null)    
        {
            if (!SlotItem.isSingle)
            {
                childText.gameObject.SetActive(true);
                Text quantityText = childText.GetComponent<Text>();
                if (quantityText != null)
                {
                    quantityText.text = SlotItem.quantity.ToString();
                }
            } 
            else childText.gameObject.SetActive(false);
        }
        else childText.gameObject.SetActive(false);    

    }
}
