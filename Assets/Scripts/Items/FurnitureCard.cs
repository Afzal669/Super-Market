using UnityEngine.UI;
using UnityEngine;

public class FurnitureCard : MonoBehaviour
{
    public GameObject Prefab;
    public Text Name;
    public Image Image;
    public Sprite sprite;
    public Text Quantity;
    public Text TotalPrice;
    public float Unit_Price = 0;
    public int quantity = 0;


    void Start()
    {
        DataSet();
    }

    public void onClickIncr()
    {
        quantity++;
        Quantity.text = quantity.ToString();
        //TotalPrice.text = "$"+(quantity * Unit_Price).ToString();
        onClickDataChange();
    }

    public void onClickDec()
    {
        if (quantity > 0)
        {
            quantity--;
            Quantity.text = quantity.ToString();
            //TotalPrice.text = "$" + (quantity * Unit_Price).ToString();
            onClickDataChange();
        }

    }

    public void onClickDataChange()
    {
        ShopManagment.instance.PushDataFurniture(Prefab, quantity);
    }
    public void DataSet()
    {
        Quantity.text = quantity.ToString();
        Unit_Price = Prefab.GetComponent<FurnishItem>().Unit_Price;
        TotalPrice.text = "$ " + Unit_Price;
        if(sprite)
        {
            Image.sprite = sprite;
        }
        
        //TotalPrice.text = "$" + (quantity * Unit_Price).ToString();
    }
    public void ResetData()
    {
        quantity = 0;
        Quantity.text = quantity.ToString();
       
    }

}
