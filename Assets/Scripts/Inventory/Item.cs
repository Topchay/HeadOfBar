using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("����� ���������� � ��������")]
    public string itemID;                              // ���������� ������������� ��������
    public string itemName = " ";                      // �������� ��������
    public string description = "�������� ��������";   // �������� ��������
    public Sprite icon = null;                         // ������ ��������
    public GameObject prefab;                          // ������ ��������                         

    [Header("��������������")]
    public bool isSingle;                              // ����������� ���� � ������������ ����������
    public int quantity = 1;                           // ���������� ���������
}