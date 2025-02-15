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

    public RectTransform[] windows; // ������ ����

    private Action placeAction;
    private Action stopAction;

    public Button[] buttons; // ������ ������

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

        // �������� ������� ������� ���� � ���������� ����������.
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
        // �������� ��������� InventorySlot �� ������
        InventorySlot inventorySlot = buttons[buttonIndex].GetComponent<InventorySlot>();
        Image spriteSlot = buttons[buttonIndex].GetComponent<Image>();

        if (inventorySlot.SlotItem != null)
        {
            gridVisualization.SetActive(true);
            cellIndicator.SetActive(true);

            GameObject prefab = inventorySlot.SlotItem.prefab;

            // ���������� ��������
            placeAction = () => PlaceStructure(prefab, inventorySlot, spriteSlot);
            stopAction = () => StopPlacement();

            // ��������� �������� � ��������
            inputManagerMouse.OnClicked += placeAction;
            inputManagerMouse.OnExit += stopAction;
        }
        else print("�����");
    }

    private void PlaceStructure(GameObject prefabSlot, InventorySlot inventorySlot, Image spriteSlot)
    {
        // ���������, ��������� �� ��������� ���� ��� ���������� UI; ���� ��, �� ������������ �� ������.
        if (inputManagerMouse.IsPointerOverUI())
        {
            return;
        }

        // �������� ������� ������� �� ����� � ����������� � � ������� �����.(�������������� ������)
        Vector3 mousePosition = inputManagerMouse.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // ������� ����� ������ �� ������ ���������� ������� � ��������� ��� � ����� c ������������� �����������
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
        // ���������� ��������� ������ ������� � ������������ ������������.
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
                // ��������, ��������� �� ������� ���������� ����� � �������� ������
                Vector2 localMousePosition = buttons[i].GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);

                if (buttons[i].GetComponent<RectTransform>().rect.Contains(localMousePosition))
                {
                    ButtonClicked(i);
                }
            }
        }
    }
}