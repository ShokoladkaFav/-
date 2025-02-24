using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
    public InventoryUI inventoryUI;
    public int maxCapacity = 20;

    public Canvas notificationCanvas;
    public TMPro.TextMeshProUGUI notificationText;
    public float notificationDuration = 2f;

    public static Inventory instance;
    public Transform inventoryUIParent;
    public GameObject itemUIPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
        }

        if (notificationCanvas == null)
        {
            notificationCanvas = FindObjectOfType<Canvas>();
        }
    }

    public void UpdateInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.UpdateUI();
        }
        else
        {
            Debug.LogWarning("InventoryUI не встановлено!");
        }
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= maxCapacity)
        {
            ShowInventoryFullNotification();
            return false;
        }

        items.Add(item);
        inventoryUI?.UpdateUI();

        return true;
    }

    public void RemoveItem(Item item)
    {
        Debug.Log($"Видаляємо {item.itemName}");
        items.Remove(item);
        Debug.Log($"Кількість предметів у інвентарі після видалення: {items.Count}");
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public int GetItemCount(Item targetItem)
    {
        int count = 0;

        foreach (Item item in items)
        {
            if (item.itemName == targetItem.itemName)
            {
                count += item.currentStackSize;
            }
        }

        return count;
    }

    private void ShowInventoryFullNotification()
    {
        if (notificationCanvas != null && notificationText != null)
        {
            notificationCanvas.gameObject.SetActive(true);
            notificationText.text = "Інвентар заповнений!";
            Invoke(nameof(HideNotification), notificationDuration);
        }
    }

    private void HideNotification()
    {
        if (notificationCanvas != null)
        {
            notificationCanvas.gameObject.SetActive(false);
        }
    }
}
