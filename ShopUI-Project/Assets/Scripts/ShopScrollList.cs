using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable] // to set values of class in unity inspector
public class Item
{
    public string itemName;
    public Sprite icon;
    public float price = 1f;
}


public class ShopScrollList : MonoBehaviour {

    public List<Item> itemList;
    public Transform contentPanel;
    public ShopScrollList otherShop; // for panels talking to each other...
    public Text myGoldDisplay;
    public SimpleObjectPool buttonObjectPool;
    public float gold = 20f;

	// Use this for initialization
	void Start () {
		
	}
	
    // adds buttons for each item stored in our item list.
    // calls setup function of sample button
    private void AddButtons()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];
            GameObject newButton = buttonObjectPool.GetObject();
        }
    }
}
