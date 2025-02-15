using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArrayHandler : MonoBehaviour
{
    Inventory Inventory;

    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManagerMouse inputManagerMouse;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject gridVisualization;

    public RectTransform[] windows; // Массив окон

    private Action placeAction;
    private Action stopAction;

    public Button[] buttons; // Массив кнопок

    private void Awake()
    {
        Inventory = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        StopPlacement();
    }

    void Update()
    {
        CheckClickPanelSlot();

        // Получает текущую позицию мыши и отображает индикаторы.
        Vector3 mousePosition = inputManagerMouse.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (Inventory.mouseMoveDrag)
        {
            StopPlacement();
        }
    }

    public void ButtonClicked(int buttonIndex)
    {
        // Получаем компонент InventorySlot из кнопки
        InventorySlot inventorySlot = buttons[buttonIndex].GetComponent<InventorySlot>();
        Image spriteSlot = buttons[buttonIndex].GetComponent<Image>();

        if (inventorySlot.SlotItem != null)
        {
            gridVisualization.SetActive(true);
            cellIndicator.SetActive(true);

            GameObject prefab = inventorySlot.SlotItem.prefab;

            // Определяем действия
            placeAction = () => PlaceStructure(prefab, inventorySlot, spriteSlot);
            stopAction = () => StopPlacement();

            // Добавляем действия к событиям
            inputManagerMouse.OnClicked += placeAction;
            inputManagerMouse.OnExit += stopAction;
        }
        else print("пусто");
    }

    private void PlaceStructure(GameObject prefabSlot, InventorySlot inventorySlot, Image spriteSlot)
    {
        // Проверяет, находится ли указатель мыши над элементами UI; если да, то возвращается из метода.
        if (inputManagerMouse.IsPointerOverUI())
        {
            return;
        }

        // Получает позицию курсора на карте и преобразует её в позицию сетки.(Предполагаемая ошибка)
        Vector3 mousePosition = inputManagerMouse.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Создает новый объект на основе выбранного индекса и размещает его в сетке c определенными параметрами
        GameObject newObject = Instantiate(prefabSlot);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        PickUpObject newPickUpObject = newObject.GetComponent<PickUpObject>();

        newPickUpObject.originalItemObj = inventorySlot.SlotItem;
        newPickUpObject.itemObj = null;


        inventorySlot.SlotItem = null;
        spriteSlot.sprite = inventorySlot.spriteEmpty;
        spriteSlot.color = new Color(spriteSlot.color.r, spriteSlot.color.g, spriteSlot.color.b, 0f);
        inventorySlot.QuantityText();

        StopPlacement();
    }

    private void StopPlacement()
    {
        // Сбрасывает выбранный индекс объекта и деактивирует визуализацию.
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);

        if (placeAction != null)
        {
            inputManagerMouse.OnClicked -= placeAction;
            placeAction = null;
        }

        if (stopAction != null)
        {
            inputManagerMouse.OnExit -= stopAction;
            stopAction = null;
        }
    }

    private void CheckClickPanelSlot()
    {
        if (Input.GetMouseButtonDown(0) && !Inventory.mouseMoveDrag)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                // Проверка, совпадает ли текущая координата клика с позицией кнопки
                Vector2 localMousePosition = buttons[i].GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);

                if (buttons[i].GetComponent<RectTransform>().rect.Contains(localMousePosition))
                {
                    ButtonClicked(i);
                }
            }
        }
    }
}