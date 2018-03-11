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
        RefreshDisplay();
	}
	
    public void RefreshDisplay()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        myGoldDisplay.text = "Gold: " + gold.ToString();
        // We remove all 
        RemoveButtons();
        // Because then we just add the ones we actually need.
        AddButtons();
        

    }
    // adds buttons for each item stored in our item list.
    // calls setup function of sample button
    private void AddButtons()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];
            GameObject newButton = buttonObjectPool.GetObject(/*contentPanel*/);
            newButton.transform.SetParent(contentPanel); // will be automatically arranged correctly given the arrangements we set up in UI

            // RELEVANT
            // WORKAROUND FOR FUCKING BUG THAT MESSES UP EVERYTHING
            // https://answers.unity.com/questions/1257149/scaling-dynamic-created-buttons.html
            newButton.transform.localScale = new Vector3(1f, 1f, 1f);
            
            
            // "Tell the button to set itself up..."
            // "So we are going to get a reference to that sample button script that we attache to our
            // sample button prefab"
            // YEAH... currently newButton stands for a gameobject...
            // SampleButton is about our kind of button for the game...
            // which is all part of the same thing but different components...
            SampleButton sampleButton = newButton.GetComponent<SampleButton>(); // reference to script...
            sampleButton.Setup(item, this);
        }
    }

    private void RemoveButtons()
    {
        while(contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }

    }

    public void TryTransferItemToOtherShop(Item item)
    {
        if (otherShop.gold >= item.price)
        {
            gold += item.price;
            otherShop.gold -= item.price;


            AddItem(item, otherShop);
            RemoveItem(item, this);

            RefreshDisplay();
            otherShop.RefreshDisplay();


        }
;
    }
    private void AddItem(Item itemToAdd, ShopScrollList shopList)
    {
        shopList.itemList.Add(itemToAdd);
        
    }

    private void RemoveItem(Item itemToRemove, ShopScrollList shopList)
    {

        // Don't really got why .Remove() is not enough.
        for (int i = shopList.itemList.Count - 1; i >= 0; i--)
        {
            if (shopList.itemList[i] == itemToRemove)
            {
                shopList.itemList.RemoveAt(i);
            }
        }
    }

}
