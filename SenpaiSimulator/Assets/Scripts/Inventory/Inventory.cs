using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ItemDatabase {

    public List<Item> items = new List<Item>();

    private void Start()
    {
        AddItem(0);
        AddItem(0);
        AddItem(1);
    }

    public void AddItem(int itemID)
    {
        Item item = Database[itemID];
        bool a = true;
        foreach (Item _item in items)
        {
            if (item.itemName == _item.itemName)
            {
                _item.quantity++;
                a = false;
                break;
            }
        }
        if (a)
        {
            items.Add(item);
        }
    }

    public void DropItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (itemName == item.itemName)
            {
                print("Found it!");
                break;
            }
        }
    }

    public Item SearchInv(string name)
    {
        Item targetItem;
        foreach (Item item in items)
        {
            if (name == item.itemName)
            {
                print("Found it!");
                targetItem = item;
                return targetItem;
            }
        }
        print("Not found");
        return null;
    }

    private void Update()
    {
    }
}
