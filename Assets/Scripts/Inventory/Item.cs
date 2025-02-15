using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Общая информация о предмете")]
    public string itemID;                              // Уникальный идентификатор предмета
    public string itemName = " ";                      // Название предмета
    public string description = "Описание предмета";   // Описание предмета
    public Sprite icon = null;                         // Иконка предмета
    public GameObject prefab;                          // Префаб предмета                         

    [Header("Характеристики")]
    public bool isSingle;                              // Возможность быть в единственном экземпляре
    public int quantity = 1;                           // Количество предметов
}