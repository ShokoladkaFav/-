using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;                      // Назва предмета
    [SerializeField] public bool isQuestItem;    // Визначає, чи є предмет квестовим
    public int costCopper;                       // Ціна в бронзових монетах
    public int costSilver;                       // Ціна в срібних монетах
    public int costGold;                         // Ціна в золотих монетах
    public Sprite itemIcon;                      // Іконка предмета
    public bool isStackable;                     // Чи можна предмет стакувати
    public int maxStackSize = 99;                // Максимальна кількість предметів у одному стаку
    public int currentStackSize = 1;             // Скільки предметів із самого початку у стаку
    public GameObject prefab;                    // Префаб предмета
    public ItemType itemType;                    // Тип предмета

    public enum ItemType
    {
        None,
        Helmet,
        Chestplate,
        Gloves,
        Pants,
        Boots,
        Weapon,
        Shield,
        Accessory
    }
}
