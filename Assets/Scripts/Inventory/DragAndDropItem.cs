using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TerrainUtils;
using UnityEngine.UI;
public class DragAndDropItem : MonoBehaviour
{
    InventorySlot InventorySlot;
    Inventory Inventory;
    ButtonArrayHandler ButtonArrayHandler;

    private Transform originalParent;
    private Canvas canvas;
    private Vector3 startPos;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        Inventory = FindObjectOfType<Inventory>();
        InventorySlot = FindObjectOfType<InventorySlot>();
        ButtonArrayHandler = FindObjectOfType<ButtonArrayHandler>();
    }

    public void MouseDown()
    {
        // Сохранить ссылку на изначального родителя
        originalParent = transform.parent;

        Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        startPos = transform.position;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    public void MouseDrag()
    {
        Inventory.mouseMoveDrag = true;

        // Переместить объект в корень канваса
        transform.SetParent(canvas.transform, true);

        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = newPosition;
    }
    public void MouseUp()
    {
        Inventory.mouseMoveDrag = false;
        // Вернуть объект обратно к исходному родителю
        transform.SetParent(originalParent, true);

        CheckCanvas();

        SlotBehavior();
    }

    private List<RaycastResult> CastRay()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> resultData = new List<RaycastResult>();
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, resultData);

        if (resultData.Count > 0)
        {
            return resultData;
        }
        return null;
    }

    public bool CheckUI()
    {
        return CastRay() != null;
    }

    private void SlotBehavior()
    {
        if (CastRay() != null && CastRay().Count > 1)
        {
            var hitObjects = CastRay();
            GameObject collidedSlot = hitObjects[1].gameObject;

            if (collidedSlot)
            {
                MergingSlots(collidedSlot);
                ReplacingSlots(collidedSlot);
            }
        }
        else
        {
            transform.position = startPos;
        }
    }

    private void MergingSlots(GameObject collidedSlot)
    {
        var currentObjectSlot = GetComponent<InventorySlot>();
        var hitObjectSlot = collidedSlot.GetComponent<InventorySlot>();

        // Объединение только в случае, если оба слота заняты
        if (currentObjectSlot.SlotItem != null && hitObjectSlot.SlotItem != null)
        {
            if (currentObjectSlot.SlotItem.itemID == hitObjectSlot.SlotItem.itemID)
            {
                if (!currentObjectSlot.SlotItem.isSingle)
                {
                    currentObjectSlot.SlotItem.quantity += hitObjectSlot.SlotItem.quantity;
                    hitObjectSlot.SlotItem = null;

                    Image iconSlot = hitObjectSlot.GetComponent<Image>();
                    iconSlot.sprite = InventorySlot.spriteEmpty;
                    iconSlot.color = new Color(iconSlot.color.r, iconSlot.color.g, iconSlot.color.b, 0f);

                    currentObjectSlot.GetComponent<InventorySlot>().QuantityText();

                    transform.position = startPos;
                    return;
                }
            }
            else return;
        }
    }

    private void ReplacingSlots(GameObject collidedSlot)
    {
        InventorySlot currentSlot = GetComponent<InventorySlot>();
        InventorySlot collidedSlots = collidedSlot.GetComponent<InventorySlot>();
        List<InventorySlot> itemList = Inventory.GetSlotList();

        if (!currentSlot.CompareTag("PaneSlots") && collidedSlot.CompareTag("InventorySlots"))         // Перетаскивание внутри инвентаря
        {
            RearrangementSlotInventory(collidedSlot);
        }
        else if (currentSlot.CompareTag("InventorySlots") && collidedSlot.CompareTag("PaneSlots"))     // Перетаскивание из инвентаря в панель
        {
            RearrangementSlotPanel(collidedSlot);
        }
        else if (currentSlot.CompareTag("PaneSlots") && collidedSlot.CompareTag("PaneSlots"))          // Перетаскивание внутри панели
        {
            RearrangementSlotPanel(collidedSlot);
        }
        else if (currentSlot.CompareTag("PaneSlots") && collidedSlot.CompareTag("InventorySlots"))     // Перетаскивание из панели в инвентарь
        {
            RearrangementSlotPanel(collidedSlot);
        }

        collidedSlot.GetComponent<InventorySlot>().QuantityText();
        currentSlot.GetComponent<InventorySlot>().QuantityText();
    }

    private void RearrangementSlotInventory(GameObject collidedSlot)
    {
        InventorySlot currentSlot = GetComponent<InventorySlot>();
        InventorySlot collidedSlots = collidedSlot.GetComponent<InventorySlot>();
        List<InventorySlot> itemList = Inventory.GetSlotList();

        int currentIndex = Inventory.GetSlotIndex(currentSlot);
        int collidedIndex = Inventory.GetSlotIndex(collidedSlots);

        Inventory.SwapItems(itemList, currentIndex, collidedIndex);

        transform.position = collidedSlot.gameObject.transform.position;
        collidedSlot.gameObject.transform.position = startPos;
    }

    private void RearrangementSlotPanel(GameObject collidedSlot)
    {
        InventorySlot currentSlot = GetComponent<InventorySlot>();
        InventorySlot collidedSlots = collidedSlot.GetComponent<InventorySlot>();
        Image currentImage = currentSlot.GetComponent<Image>();
        Image collidedImage = collidedSlots.GetComponent<Image>();

        var current = currentSlot.SlotItem;
        var collided = collidedSlots.SlotItem;

        Sprite curImage = currentImage.sprite;
        Sprite colImage = collidedImage.sprite;

        if (currentSlot.SlotItem == null) { transform.position = startPos; }

        else if (collidedSlots.SlotItem == null)
        {
            collidedSlots.SlotItem = currentSlot.SlotItem;
            currentSlot.SlotItem = null;

            collidedImage.sprite = currentImage.sprite;
            currentImage.sprite = InventorySlot.spriteEmpty;
            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 0f);
            collidedImage.color = new Color(collidedImage.color.r, collidedImage.color.g, collidedImage.color.b, 1f);
            transform.position = startPos;
        }
        else
        {
            currentSlot.SlotItem = collided;
            collidedSlots.SlotItem = current;

            currentImage.sprite = colImage;
            collidedImage.sprite = curImage;

            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1f);
            collidedImage.color = new Color(collidedImage.color.r, collidedImage.color.g, collidedImage.color.b, 1f);
            transform.position = startPos;
        }
    }

    public void CheckCanvas()
    {
        RectTransform objRectTransform = GetComponent<RectTransform>();

        foreach (RectTransform window in ButtonArrayHandler.windows)
        {
            if (IsObjectInsideWindow(objRectTransform, window))
            {
                Debug.Log("Объект внутри окна: " + window.name);
            }
            else
            {
                Debug.Log("Объект вне окна: " + window.name);
            }
        }
    }

    private bool IsObjectInsideWindow(RectTransform objRect, RectTransform windowRect)
    {
        Vector3[] objCorners = new Vector3[4];
        Vector3[] windowCorners = new Vector3[4];

        objRect.GetWorldCorners(objCorners);
        windowRect.GetWorldCorners(windowCorners);

        foreach (Vector3 corner in objCorners)
        {
            if (!PointInRectangle(corner, windowCorners))
            {
                return false;
            }
        }

        return true;
    }

    private bool PointInRectangle(Vector3 point, Vector3[] rectCorners)
    {
        return (point.x >= rectCorners[0].x && point.x <= rectCorners[2].x &&
                point.y >= rectCorners[0].y && point.y <= rectCorners[2].y);
    }
}