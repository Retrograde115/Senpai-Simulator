using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Inventory inventory;
    public InventoryUI inventoryUI;
    Item targetItem;

    public void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void SelectItem(string item)
    {
        item = GetComponentInChildren<Text>().text;
        print(item);
        targetItem = inventory.SearchInv(item);
        inventoryUI.Select(targetItem);
    }
}
