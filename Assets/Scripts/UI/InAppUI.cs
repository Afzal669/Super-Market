using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InAppUI : MonoBehaviour
{
    public static InAppUI instance;
    public int currentVal = 0;
    public List<Sprite> Icons;
    public List<int> Prices;

    public Image image;
    public Text price;
    private void Awake()
    {
        instance = this;
    }
    public void ShowData(int val)
    {
        currentVal = val;
        print(currentVal);
        ShowImagesAndPrice();
    }

    public void ShowImagesAndPrice()
    {
        image.sprite = Icons[currentVal];
        price.text = "YOU RECEIVED! " + Prices[currentVal].ToString();
    }

    private void latCall()
    {
        image.sprite = Icons[currentVal];
        price.text = "YOU RECEIVED! " + Prices[currentVal].ToString();
    }
}
