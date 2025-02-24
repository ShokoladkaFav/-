using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // ��������� �� �������� ������
    public GameObject slotPrefab; // ������ �����
    public Transform slotsParent; // ����������� ��'��� ��� �����

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // ������� ���� �����
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        // ������ ������� ������� ��������
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

        // ��������� ����� ��� ������� ���� ��������
        foreach (var kvp in itemCounts)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);

            // ������������ ������
            Image icon = newSlot.transform.Find("Icon").GetComponent<Image>();
            Item firstItem = inventory.items.Find(x => x.itemName == kvp.Key);
            if (icon != null && firstItem.itemIcon != null)
            {
                icon.sprite = firstItem.itemIcon;
            }

            // ������������ �������
            Text quantityText = newSlot.transform.Find("QuantityText").GetComponent<Text>();
            if (quantityText != null)
            {
                quantityText.text = kvp.Value > 1 ? kvp.Value.ToString() : "";
            }

            // ������������ ����� ��������
            Text itemNameText = newSlot.transform.Find("ItemNameText").GetComponent<Text>();
            if (itemNameText != null)
            {
                itemNameText.text = kvp.Key.Length > 10 ? kvp.Key.Substring(0, 10) + "..." : kvp.Key; // ���������� �����
            }

            // ������ ���������� ��� ����, ��������� ���������� �������
            AddClickFunctionality(newSlot, firstItem);
        }
    }
    private void AddClickFunctionality(GameObject slot, Item item)
    {
        EventTrigger eventTrigger = slot.AddComponent<EventTrigger>();

        // ������� ����� ���� (���������� � ���� �����������)
        EventTrigger.Entry leftClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        leftClickEntry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Left) // ˳�� ������ ����
            {
                EquipItem(item);
            }
        });
        eventTrigger.triggers.Add(leftClickEntry);

        // ������� ������� ���� (��������� ��������)
        EventTrigger.Entry rightClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        rightClickEntry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Right) // ����� ������ ����
            {
                inventory.RemoveItem(item); // ��������� ������� �� ���������
                UpdateUI(); // ��������� UI ���� ���������
            }
        });
        eventTrigger.triggers.Add(rightClickEntry);
    }

    private void EquipItem(Item item)
    {
        // ��������� ���� ��� �����������
        EquipmentSlot slot = inventory.equipmentSlots.Find(s => s.slotType == item.itemType);
        if (slot != null)
        {
            // ���� ���� ��������, ��������� ��������� ������� � ��������
            if (slot.currentItem != null)
            {
                if (!inventory.AddItem(slot.currentItem))
                {
                    Debug.Log("�������� ����������!");
                    return;
                }
            }

            // ������ ������� � ����
            slot.SetItem(item);
            inventory.RemoveItem(item); // ��������� ������� �� ���������
            UpdateUI(); // ��������� UI
        }
        else
        {
            Debug.Log("���� ���������� ����� ��� ����� ��������.");
        }
    }
}