using UnityEngine.UI;
using UnityEngine;

public class CardItem : MonoBehaviour
{
    public enum Type
    {
        Shelf,
        Fridge
    }
    public GameObject TopParent;
    public Text Name;
    public Image Image;
    public Text Quantity;
    public Text TotalPrice;
    public float Unit_Price = 0;
    public int Space = 0;
    public int Display = 0;

    [Header("Others")]
    public GameObject PrefrabsItem;
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
        if(quantity > 0)
        {
            quantity--;
            Quantity.text = quantity.ToString();
            //TotalPrice.text = "$" + (quantity * Unit_Price).ToString();
            onClickDataChange();
        }
        
    }

    public void onClickDataChange()
    {
        ShopManagment.instance.PushData(PrefrabsItem, quantity);
    }
    public void DataSet()
    {
        Quantity.text = quantity.ToString();
        TotalPrice.text = "$ " + Unit_Price;
        //TotalPrice.text = "$" + (quantity * Unit_Price).ToString();
    }
}
