using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CardItem : MonoBehaviour
{
    public GameObject TopParent;
    public TextMeshProUGUI Name;
    public Image Image;
    public Sprite ImageSprite;
    public TextMeshProUGUI Display;
    public TextMeshProUGUI Unit_Price;
    public TextMeshProUGUI Count;

    [Header("Amount")]
    public TextMeshProUGUI Quantity;
    public TextMeshProUGUI TotalPrice;

    [Header("Others")]
    public GameObject PrefrabsItem;
    void Start()
    {

    }

    public void onClickIncr()
    {

    }

    public void onClickDec()
    {

    }

    public void onClickAddToCart()
    {

    }
}
