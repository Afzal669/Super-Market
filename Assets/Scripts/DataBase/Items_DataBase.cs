using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_DataBase : MonoBehaviour
{
    public enum Type
    {
        Shelf,
        Fridge
    }
    [System.Serializable]
    public class ItemsData
    {
        public string Name;
        public Sprite Sprite;
        public float UnitPrice;
        public int Space;
        public Type Display;
        public GameObject Prefab;
    }

    public GameObject Card,currentCard;
    public Transform position;
    public CardItem cardItemScritp;

    public List<ItemsData> DataBase;

    public List<GameObject> AllItems;
    public item itemScript;
    public static Items_DataBase Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {

        foreach(var obj in AllItems)
        {
            ItemsData itemsData = new ItemsData();
            itemScript = obj.GetComponent<item>();
            itemsData.Name = obj.name;
            itemsData.Sprite = itemScript.sprite;
            itemsData.UnitPrice = itemScript.Unit_Price;
            itemsData.Space = itemScript.Space;
            if(itemScript.Display == item.Type.Fridge)
            {
                itemsData.Display = Type.Fridge;
            }else
            {
                itemsData.Display = Type.Shelf;
            }
            itemsData.Prefab = itemScript.Self;
            DataBase.Add(itemsData);
        }

        foreach(var item in DataBase)
        {
            currentCard = Instantiate(Card, position);
            cardItemScritp = currentCard.GetComponent<CardItem>();
            cardItemScritp.Name.text = item.Name;
            cardItemScritp.Image.sprite = item.Sprite;
            cardItemScritp.Unit_Price = item.UnitPrice * 12f;
            cardItemScritp.Space = item.Space;
            cardItemScritp.PrefrabsItem = item.Prefab;
            if(item.Display == Type.Fridge)
            {
                cardItemScritp.Display = 1;
            }else
            {
                cardItemScritp.Display = 0;
            }
            ShopManagment.instance.AllProductsCardsCounters.Add(currentCard);

        }
    }

   
}
