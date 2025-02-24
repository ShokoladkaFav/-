using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // Посилання на інвентар гравця
    public GameObject slotPrefab; // Префаб слоту
    public Transform slotsParent; // Батьківський об'єкт для слотів

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Очищуємо старі слоти
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        // Рахуємо кількість кожного предмета
        foreach (Item item in inventory.items)
        {
            if (itemCounts.ContainsKey(item.itemName))
            {
                itemCounts[item.itemName]++;
            }
            else
            {
                itemCounts[item.itemName] = 1;
            }
        }

        // Створюємо слоти для кожного типу предметів
        foreach (var kvp in itemCounts)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);

            // Встановлення іконки
            Image icon = newSlot.transform.Find("Icon").GetComponent<Image>();
            Item firstItem = inventory.items.Find(x => x.itemName == kvp.Key);
            if (icon != null && firstItem.itemIcon != null)
            {
                icon.sprite = firstItem.itemIcon;
            }

            // Встановлення кількості
            Text quantityText = newSlot.transform.Find("QuantityText").GetComponent<Text>();
            if (quantityText != null)
            {
                quantityText.text = kvp.Value > 1 ? kvp.Value.ToString() : "";
            }

            // Встановлення назви предмета
            Text itemNameText = newSlot.transform.Find("ItemNameText").GetComponent<Text>();
            if (itemNameText != null)
            {
                itemNameText.text = kvp.Key.Length > 10 ? kvp.Key.Substring(0, 10) + "..." : kvp.Key; // Скорочення назви
            }

            // Додаємо функціонал для кліків, передаючи правильний предмет
            AddClickFunctionality(newSlot, firstItem);
        }
    }
    private void AddClickFunctionality(GameObject slot, Item item)
    {
        EventTrigger eventTrigger = slot.AddComponent<EventTrigger>();

        // Обробка лівого кліку (переміщення у слот спорядження)
        EventTrigger.Entry leftClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        leftClickEntry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Left) // Ліва кнопка миші
            {
                EquipItem(item);
            }
        });
        eventTrigger.triggers.Add(leftClickEntry);

        // Обробка правого кліку (видалення предмета)
        EventTrigger.Entry rightClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        rightClickEntry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Right) // Права кнопка миші
            {
                inventory.RemoveItem(item); // Видаляємо предмет із інвентаря
                UpdateUI(); // Оновлюємо UI після видалення
            }
        });
        eventTrigger.triggers.Add(rightClickEntry);
    }

    private void EquipItem(Item item)
    {
        // Знаходимо слот для спорядження
        EquipmentSlot slot = inventory.equipmentSlots.Find(s => s.slotType == item.itemType);
        if (slot != null)
        {
            // Якщо слот зайнятий, повертаємо попередній предмет в інвентар
            if (slot.currentItem != null)
            {
                if (!inventory.AddItem(slot.currentItem))
                {
                    Debug.Log("Інвентар заповнений!");
                    return;
                }
            }

            // Додаємо предмет у слот
            slot.SetItem(item);
            inventory.RemoveItem(item); // Видаляємо предмет із інвентаря
            UpdateUI(); // Оновлюємо UI
        }
        else
        {
            Debug.Log("Немає відповідного слота для цього предмета.");
        }
    }
}