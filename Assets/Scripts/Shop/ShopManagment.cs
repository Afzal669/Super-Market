using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class ShopManagment : MonoBehaviour
{
    public bool isStoreOpen = false;
    public GameObject Box, PositionToInst;
    public float TotalPrice = 0;
    public bool isInProductMood = true;
    public List<CartItems> cartItemsList;
    public List<CartItems> cartItemsListFurnish;

    public static ShopManagment instance;

    [Serializable]
    public class CartItems
    {
        public GameObject item;
        public int Quantity;
    }

   
    private void Awake()
    {
        instance = this;
        cartItemsList = new List<CartItems>();
    }
    void Start()
    {
        isStoreOpen = true;
    }


    #region Products
    internal void PushData(GameObject prefrabsItem, int quantity)
    {
        // Check if the item already exists in the cart
        CartItems existingItem = cartItemsList.FirstOrDefault(c => c.item.name == prefrabsItem.name);

        if (existingItem == null)
        {
            // Item doesn't exist, so add it
            CartItems cartItems = new CartItems();
            cartItems.item = prefrabsItem;
            cartItems.Quantity = quantity;
            cartItemsList.Add(cartItems);
        }
        else
        {
            existingItem.Quantity = quantity;
        }
        UpdatePriceData();

    }

    private void UpdatePriceData()
    {
        TotalPrice = 0;
        item i=null;
        float u = 0,t = 0, q = 0;
        foreach(var item in cartItemsList)
        {
            q = item.Quantity; 
            i = item.item.GetComponent<item>();
            u = i.Unit_Price;
            t = u * q;
            TotalPrice += t;
        }
        Ui_Manager.instance.TotalPrice.text = TotalPrice.ToString()+" $";
        
    }
    #endregion


    #region Furniture
    internal void PushDataFurniture(GameObject prefrabsItem, int quantity)
    {
        // Check if the item already exists in the cart
        CartItems existingItem = cartItemsListFurnish.FirstOrDefault(c => c.item.name == prefrabsItem.name);

        if (existingItem == null)
        {
            // Item doesn't exist, so add it
            CartItems cartItems = new CartItems();
            cartItems.item = prefrabsItem;
            cartItems.Quantity = quantity;
            cartItemsListFurnish.Add(cartItems);
        }
        else
        {
            existingItem.Quantity = quantity;
        }
        UpdatePriceDataFurnish();

    }

    private void UpdatePriceDataFurnish()
    {
        TotalPrice = 0;
        FurnishItem i = null;
        float u = 0, t = 0, q = 0;
        foreach (var item in cartItemsListFurnish)
        {
            q = item.Quantity;
            i = item.item.GetComponent<FurnishItem>();
            u = i.Unit_Price;
            t = u * q;
            TotalPrice += t;
        }
        Ui_Manager.instance.TotalPrice.text = TotalPrice.ToString() + " $";

    }
    #endregion


    public void OnClickBuyBtn()
    {
        GameObject box = null;
        if(Player.instance.PlayerAmount > TotalPrice)
        {
            if(isInProductMood)
            {
                foreach (var item in cartItemsList)
                {
                 
                     GenrateBox.Instance.CreateBox(item.item, item.Quantity);
   
                }
            }else
            {
                // Furnish
                foreach (var item in cartItemsListFurnish)
                {
                    for(int i=0; i<item.Quantity;i++)
                    {
                        box = Instantiate(Box, PositionToInst.transform);
                        box.name = "Box";
                        box.GetComponent<Box>().item = item.item;
                        //box.GetComponent<Box>().count = item.Quantity;
                    }
                    
                }
            }
            
        }
        else
        {
            print("Low Amount");
        }
        
    }

    public void InProductMood()
    {
        isInProductMood = true;
        UpdatePriceData();
    }

    public void InFirnitureMood()
    {
        isInProductMood = false;
        UpdatePriceDataFurnish();
    }
}
