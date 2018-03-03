using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public GameObject itemPrefabUI;
    public Transform clippingPanel;
    public Transform itemInformation;
    public float scrollMultiplier = 5;
    public Inventory inv;
    public Animator animator;
    ManagedUpdate updater;
    bool isOpen = false;
    

    public List<GameObject> UIitems = new List<GameObject>();

    private void Start()
    {
        UpdateList();
        UpdateManager.AddManagedUpdate(this, updater);
    }

    public void UpdateThis ()
    {
        switch (Player.Instance.state)
        {
            case (Player.State.FREE):
                if (Input.GetKeyDown(KeyCode.I))
                {
                    OpenInv();
                    Player.Instance.state = Player.State.INVENTORY;
                }
                break;

            case (Player.State.INVENTORY):
                ScrollList();
                if (Input.GetKeyDown(KeyCode.I))
                {
                    CloseInv();
                    Player.Instance.state = Player.State.FREE;
                }
                break;
        }
    }

    public void OpenInv()
    {
        animator.SetBool("isOpen", true);
    }

    public void CloseInv()
    {
        animator.SetBool("isOpen", false);
    }

    void ScrollList()
    {
        foreach (GameObject obj in UIitems)
        {
            obj.GetComponent<RectTransform>().anchoredPosition += Input.mouseScrollDelta * scrollMultiplier;
        }

        if (UIitems[0].GetComponent<RectTransform>().anchoredPosition.y > 241)
        {
            int i = 0;
            foreach (GameObject obj in UIitems)
            {
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 241 + i * -42);
            }
        }
    }

    void UpdateList()
    {
        int i = 0;
        Transform k, l;
        foreach (Item item in inv.items)
        {
            GameObject j = Instantiate(itemPrefabUI, clippingPanel);
            k = j.transform.GetChild(0);
            l = j.transform.GetChild(1);
            k.GetComponent<Text>().text = item.itemName;
            l.GetComponent<Text>().text = "x" + item.quantity.ToString();
            j.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 241 + i * -42);
            UIitems.Add(j);
            i++;
        }
    }

    public void Select(Item item)
    {
        Transform i = itemInformation.GetChild(1);
        Transform j = itemInformation.GetChild(2);
        Transform k = i.GetChild(0);
        Transform l = j.GetChild(0);
        k.GetComponentInChildren<Text>().text = item.itemName;
        l.GetComponentInChildren<Text>().text = item.itemDesc;
    }

}
