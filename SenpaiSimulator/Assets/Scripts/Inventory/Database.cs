using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Allows for use of the UI
using UnityEngine.UI;


public class Database : MonoBehaviour
{

    //New list of items
    public List<ITEM> items = new List<ITEM>();

}

//Seperate class for all the different items.
[System.Serializable]
public class ITEM
{
    public int ID;
    public string name;
    public Sprite image;
}