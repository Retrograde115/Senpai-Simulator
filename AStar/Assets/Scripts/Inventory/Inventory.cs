﻿using System.Collections;
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

    //Need to translate the screen worldpoint.
    public Camera cam;
    //We need to know what button is pressed.
    public EventSystem ES;

    //This is the object UI element that is being moved on the screen.
    public RectTransform MovingObject;
    //This adds a bit off ofset to the UI element being moved around.
    public Vector3 offset;

    //Not sure what this is doing, check the void Update chunk.
    public GameObject Background;

    //This two are for the item pickup.
    private RaycastHit hit;
    //I don't know what cullingamsk does.
    public LayerMask cullingmask;

    //We also need to know the name of the object (item) pressed.
    //ID of item changed from slot.
    public int CurrentID;
    //Item held in the mouse.
    public ItemInventory currentitem;

	// Use this for initialization
	void Start ()
    {
        //Do we have something in the list yet? Aka, do we have an item? 0 means nothing.
        if (items.Count == 0)
            AddGraphics();

        //This is just spawning random shit for testing and proof of concept. Obviously won't be like this in the real thing.
        //Checking each slot in the inventory.
        for (int i = 0; i < Maxcount; i++)
        {
            //i is the slot location.
            //0, data.items.Count allows us to chose anything within the list.
            //The 100 is "health". I'm not sure what it's for, especially since he said we aren't using it.
            AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(1, 99), 100);
        }
        UpdateInventory();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //CurrentID = Item currently being moved. -1 = Not being moved.
        if (CurrentID != -1)
            MoveObject();

        //In the moment that you are holding a keydown, in this case, I.
        if(Input.GetKeyDown(KeyCode.I))
        {
            //So if active, it will no longer be active and vice versa.
            Background.SetActive(!Background.activeSelf);
            if (Background.activeSelf)
                //Updates the inventory so that when you pick up or drop an item, the inventory is correct.
                UpdateInventory();
        }
        //Press E to pick up an item.
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Check camera, has to be forward facing, check to see if something is hit, 3 unit range.
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3, cullingmask))
            {
                //Because we're hitting something, is it the right item?
                if(hit.transform.tag == "Item")
                {
                    //Access all the variables in the ItemSelf script.
                    ItemSelf newitem = hit.transform.GetComponent<ItemSelf>();
                    //Add more variables for newitem as needed.
                    SearchForSameItem(data.items[newitem.ID], newitem.count, newitem.health);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
	}

    //This allows stacking of same item types by searching and comparing all item types.
    public void SearchForSameItem(ITEM item, int count, float health)
    {
        //Checking to make sure that shit isn't zero.
        for(int i = 0; i < Maxcount; i++)
        {
            //Is the ID of the item the same?
            if(items[i].ID == item.ID)
            {
                //The count can be changed to whatever. I'm making it 99.
                //If the number exceeds the count number, can't add to the stack.
                if(items[i].count < 99)
                {
                    //Add the items.
                    items[i].count += count;
                    //This deals with the excess.
                    if(items[i].count > 99)
                    {
                        //The excess goes into the next loop.
                        count = items[i].count - 99;
                        //And then sets the stack at 99
                        items[i].count = 99;
                    }
                    else
                    {
                        count = 0;
                        //This ends the for loop because Maxcount is no longer greater than i.
                        i = Maxcount;
                    }
                }
            }
        }

        //If the count is still more than zero, means that the items don't have the same ID.
        if(count > 0)
        {
            //This for loop searches for the first empty.
            for(int i = 0; i < Maxcount; i++)
            {
                //If shit is empty
                if(items[i].ID == 0)
                {
                    AddItem(i, item, count, health);
                    i = Maxcount;
                }
            }
        }
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

            //A button. Not currently implemented yet.
            Button tempButton = newItem.GetComponent<Button>();

            //Created whenever there's a click on the designated object.
            tempButton.onClick.AddListener(delegate { SelectObject(); });

            items.Add(II);
        }
    }

    public void UpdateInventory()
    {
        //Updating the inventory each time you open it,
        for(int i = 0; i < Maxcount; i++)
        {
            //If shit isn't empty then show the actual count.
            if(items[i].ID != 0 && items[i].count > 1)
            {
                //The actual string you see.
                items[i].ItemGameObj.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
            else
            {
                items[i].ItemGameObj.GetComponentInChildren<Text>().text = "";
            }

            items[i].ItemGameObj.GetComponent<Image>().sprite = data.items[items[i].ID].image;

        }
    }

    public void SelectObject()
    {
        //If there are no items in what you're trying to grab.
        if(CurrentID == -1)
        {
            //currentID is the same as the pressed button.
            //ES = Event System
            //currentSelectedGameObject is the button that was just pressed. SelectObject is called at this time.
            //int.Parse is changing the str to an int.
            CurrentID = int.Parse(ES.currentSelectedGameObject.name);
            //The inventory slots.
            currentitem = CopyInventoryItem(items[CurrentID]);

            //Want to be able to see the object being moved around.
            MovingObject.gameObject.SetActive(true);
            //Get the sprite of the image being moved.
            MovingObject.GetComponent<Image>().sprite = data.items[currentitem.ID].image;

            //Need to add the item. But the old slot should now be empty. 0 = empty. 0,0 = health also zero.
            AddItem(CurrentID, data.items[0], 0,0);
        }
        else
        {
            //This deals with stacking items of the same type.
            ItemInventory II = items[int.Parse(ES.currentSelectedGameObject.name)];

            //If current item isn't the same
            if(currentitem.ID != II.ID)
            {
                AddInventoryItem(CurrentID, II);

                //Need to add the new spot to the old spot
                AddInventoryItem(int.Parse(ES.currentSelectedGameObject.name), currentitem);
            }
            else
            {
                //If less than 99, add it.
                if(II.count + currentitem.count <= 99)
                {
                    II.count += currentitem.count;
                }
                //Add what's left.
                else
                {
                    //Calculating what is left.
                    AddItem(CurrentID, data.items[II.ID], II.count + currentitem.count - 99, II.health);
                    //64 is the max in each slot so just set it at this
                    II.count = 64;
                }

                //Update object text for item number.
                II.ItemGameObj.GetComponentInChildren<Text>().text = II.count.ToString();
            }

            //Now the old spot is empty. Essentially a reset.
            CurrentID = -1;

            //Can't see the moved object anymore.
            MovingObject.gameObject.SetActive(false);
        }
    }

    public void MoveObject()
    {
        //Establish the position of the object. Offset allows us to change the mouse position while in-game.
        Vector3 pos = Input.mousePosition + offset;
        //Reset the z variable or depth which isn't being used.
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        //Set the position towards the camera and where the object is.
        MovingObject.position = cam.ScreenToWorldPoint(pos);
    }

    //This makes a copy of the inventory
    public ItemInventory CopyInventoryItem(ItemInventory old)
    {
        //Makes the new inventory. New needs to be capitalized to work.
        ItemInventory New = new ItemInventory();

        //Copypaste all the other IDs that we are using here.
        New.ID = old.ID;
        New.ItemGameObj = old.ItemGameObj;
        New.health = old.health;
        New.count = old.count;

        return New;
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