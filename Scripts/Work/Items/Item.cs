using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;                      // ����� ��������
    [SerializeField] public bool isQuestItem;    // �������, �� � ������� ���������
    public int costCopper;                       // ֳ�� � ��������� �������
    public int costSilver;                       // ֳ�� � ������ �������
    public int costGold;                         // ֳ�� � ������� �������
    public Sprite itemIcon;                      // ������ ��������
    public bool isStackable;                     // �� ����� ������� ���������
    public int maxStackSize = 99;                // ����������� ������� �������� � ������ �����
    public int currentStackSize = 1;             // ������ �������� �� ������ ������� � �����
    public GameObject prefab;                    // ������ ��������
    public ItemType itemType;                    // ��� ��������

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
