using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string itemName;
    public string itemDesc;
    public int ID;
    public int quantity = 1;

    public Item(string name)
    {
        itemName = name;
        itemDesc = "No description.";
    }

    public Item(string name, string desc)
    {
        itemName = name;
        itemDesc = desc;
    }
}
