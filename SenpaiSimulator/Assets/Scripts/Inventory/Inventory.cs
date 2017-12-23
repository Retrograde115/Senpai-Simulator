using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Still using the UI.
using UnityEngine.UI;
//Various buttons may need various events when pressed.
using UnityEngine.EventSystems;


public class Inventory : MonoBehaviour

{
    //Link to the database with all the items in it.
    public Database data;

    //Has all the different slots that we're going to have.
    public List<ItemInventory> items = new List<ItemInventory>();

    //Gameobjectshow is the actual game prefab.
    public GameObject Gameobjectshow;

    //Here we ask the parent for all the game objects.
    public GameObject InventoryMainObject;

    //Max inventory slots
    public int Maxcount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //All the potential classes of items you could have in the inventory.
    public void AddItem(int ID, ITEM item, int count, float health)
    {
        //Setting IDs
        items[ID].ID = item.ID;
        items[ID].health = health;
        items[ID].count = count;
        //This deals with the item sprite
        //The .sprite part is the image you see.
        items[ID].ItemGameObj.GetComponent<Image>().sprite = item.image;
        //Prevents double imaging but ensures that an item image is wanted.
        if (count > 1 && item.ID != 0)
            //.text is the string you see.
            items[ID].ItemGameObj.GetComponentInChildren<Text>().text = count.ToString();
        //If less than one or empty.
        else
            //"" is nothing.
            items[ID].ItemGameObj.GetComponentInChildren<Text>().text = "";
    }

    //Here we can copy one inventory item from one slot to another.
    public void AddInventoryItem(int ID, ItemInventory inv_item)
    {
        //Setting IDs
        items[ID].ID = inv_item.ID;
        items[ID].health = inv_item.health;
        items[ID].count = inv_item.count;
        //Grabbing shit from the database.
        items[ID].ItemGameObj.GetComponent<Image>().sprite = data.items[inv_item.ID].image;
        //Prevents double imaging but ensures that an item image is wanted.
        if (inv_item.count > 1 && inv_item.ID != 0)
            //.text is the string you see.
            items[ID].ItemGameObj.GetComponentInChildren<Text>().text = inv_item.count.ToString();
        //If less than one or empty.
        else
            //"" is nothing.
            items[ID].ItemGameObj.GetComponentInChildren<Text>().text = "";
    }

    //This are the graphics for items within the inventory I think. ~6:00 within the video.
    public void AddGraphics()
    {
        //Counting inventory items.
        for(int i = 0; i < Maxcount; i++)
        {
            //Instantiates the object. Makes the parent of the InventoryMainObject object.
            GameObject newItem = Instantiate(Gameobjectshow, InventoryMainObject.transform) as GameObject;

            //Hard to hear through the accent. I believe this allows us to click an item and see how many items are populated in the list?
            newItem.name = i.ToString();

            //This makes the system create the list for us, as opposed to by hand.
            ItemInventory II = new ItemInventory();

            II.ItemGameObj = newItem;

            //Sets items up for us in the UI? ~9:15 in the video.
            RectTransform RT = newItem.GetComponent<RectTransform>();
            RT.localPosition = new Vector3(0, 0, 0);
            //Prevents scale from changing.
            RT.localScale = new Vector3(1, 1, 1);
            //Text to show many objects you have.
            //Scaling is set to 1,1,1.
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            //A button.
            Button tempButton = newItem.GetComponent<Button>();

            //Addevent selectobject ->This will be used in the next tutorial.

            items.Add(II);
        }
    }

}

//Item inventory system
[System.Serializable]
public class ItemInventory
{
    public int ID;
    //Not clear but apparently this has nothing to do with prefabs? Refer to ~4:00 within the video.
    public GameObject ItemGameObj;
    //This is just to show how we can use a variable system. Nothing to do with damage.
    public float health;
    public int count;
}