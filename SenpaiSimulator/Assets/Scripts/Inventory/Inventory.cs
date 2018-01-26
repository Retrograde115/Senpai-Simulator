using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Item[] inventory = new Item[25];

    Item GetItem(int slot)
    {
        Item item = inventory[slot];
        return item;
    }

    void SetItem(int slot, Item item)
    {
        inventory[slot] = item;
    }
}
