using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EditPrice : MonoBehaviour
{
    public Text buyingPrice;
    public InputField sellingPrice;
    public Text competitivePrice;
    public Text sellingProfit;
    public Text sellingProfitText;
    public Text Name;
    public Image image;
    public float profit = 0;
    public GameObject button;
    public GameObject Warningpanel;
    public Text Warningpaneltext;
    private void Start()
    {
        // Add a listener for when the value changes in the InputField
        //sellingPrice.text = 
        sellingPrice.onValueChanged.AddListener(OnInputFieldChanged);
    }

    // This method will be called whenever the value of the InputField changes
    private void OnInputFieldChanged(string newValue)
    {
        Debug.Log("Input Field value changed: " + newValue);
        // Call your desired function here
        CustomFunction(newValue);
    }

    // Example function that you want to call
    private void CustomFunction(string value)
    {
        float result = 0;
        if (value != null)
        {
            if (float.TryParse(value, out result))
            {
                Debug.Log("Custom function called with value: " + value);
                profit = float.Parse(sellingPrice.text.ToString()) - float.Parse(buyingPrice.text.ToString());
                if (profit > 0)
                {
                    sellingProfit.text = profit.ToString();
                    sellingProfit.color = Color.green;
                    sellingProfitText.text = " PROFIT";
                }
                else
                {
                    sellingProfit.text = profit.ToString();
                    sellingProfit.color = Color.red;
                    sellingProfitText.text = " LOSS";
                }
            }
            else
            {
                Debug.Log("Input string is not in a correct format.");
            }

           
        }
        
    }

    // Remember to remove listener when the object is destroyed
    private void OnDestroy()
    {
        sellingPrice.onValueChanged.RemoveListener(OnInputFieldChanged);
    }

    public void OnClickOkey()
    {

        float per = (float.Parse(buyingPrice.text.ToString()) * 15) / 100;
        float val = per + float.Parse(buyingPrice.text.ToString());
        if (float.Parse(sellingPrice.text.ToString()) > val)
        {
            Warningpaneltext.text = "Your Selling price is \n Greater than Market \n Competitive Price";
            Warningpanel.SetActive(true);
        }
        else
        {
            print("Good");

            Player.instance.EditPricePanal.SetActive(false);
            GameObject Current_Interated_Obj = Player.instance.Current_Interated_Obj;

            GameObject parent = Current_Interated_Obj.transform.parent.gameObject;
            GameObject child = Current_Interated_Obj.transform.GetChild(0).gameObject;

            parent.GetComponent<ShelfPlacement>().GrossPrice = float.Parse(sellingPrice.text.ToString());
            child.GetComponent<Tag>().price.text = "$" + sellingPrice.text;

            parent.GetComponent<ShelfPlacement>().SetGrossValue(parent.GetComponent<ShelfPlacement>().GrossPrice);
        }

        
         
    }
}
