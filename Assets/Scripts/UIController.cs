using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup inventoryPanel;

    private void Start()
    {
        CloseInventory();
    }

    public void OpenInventory()
    {
        inventoryPanel.alpha = 1;
        inventoryPanel.interactable = true;
        inventoryPanel.blocksRaycasts = true;
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.alpha = 0;
            inventoryPanel.interactable = false;
            inventoryPanel.blocksRaycasts = false;
        }
    }
}
