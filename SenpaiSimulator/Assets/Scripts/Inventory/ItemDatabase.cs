using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public Item[] Database { get; private set; }

    private void Awake()
    {
        Database = new Item[64];
        Database[0] = new Item("Dildo", "An instrument used for insertive sexual pleasure.");
        Database[1] = new Item("Dragon Dildo", "An instrument used for insertive sexual pleasure. Dragon sized.");
    }
}

