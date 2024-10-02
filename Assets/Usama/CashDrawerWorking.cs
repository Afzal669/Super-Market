using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashDrawerWorking : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI ReceivedAmountText;
    public TextMeshProUGUI GivingAmountText;
    public TextMeshProUGUI TotalAmount;
    public TextMeshProUGUI ChangeText;
    [Header("Amounts")]
    public float Change=0;
    public float ReceivedAmount = 0; 
    public float GivingAmount=0;
    [Header("DrawerAnim")]
    public Cashdrawer CashDrawer;
    [Header("Button")]
    private Button ConfirmPaymentButton;
    public GameObject CashRegisterCanvas;
    public GameObject CustomerBillCanvas;

    // OnClick User Money Update Value
    public void UpdateCashPanel(float total,Button ConfirmPayment)
    {
        CashDrawer.Draweropen();
        // Generate a random amount greater than total and a multiple of 5
        int minMultiplier = Mathf.CeilToInt(total / 5) + 1; // Minimum multiplier to ensure greater than total
        int maxMultiplier = minMultiplier +2; // Max multiplier range (adjust as needed)
        int randomMultiplier = Random.Range(minMultiplier, maxMultiplier); // Generate a random multiplier
        // Calculate the received amount as a multiple of 5
        ReceivedAmount = randomMultiplier * 5;
        total = Mathf.Round(total * 100f) / 100f;  // Round to 2 decimal places
        ReceivedAmountText.text ="$"+ ReceivedAmount.ToString("F2");
        TotalAmount.text = "$" + total.ToString("F2");
        Change = ReceivedAmount - total;
        Change = Mathf.Round(Change * 100f) / 100f;  // Round to 2 decimal places
        ChangeText.text = "$" + Change.ToString("F2"); // Ensure the change is displayed with 2 decimal places
        ConfirmPayment.gameObject.SetActive(true);
        ConfirmPaymentButton = ConfirmPayment;
        ConfirmPayment.onClick.AddListener(ConfirmGivingPayment);
        CashRegisterCanvas.SetActive(true);
        CustomerBillCanvas.SetActive(false);
    }
    // OnClick Money Of Counter Add to Giving Money And Update
    public void UpdateGivingAmount(float AddAmount)
    {
        float Amount = GivingAmount;
        Amount += AddAmount;
        if (Amount > 0)
        {
            GivingAmount = Amount;
            GivingAmountText.text = GivingAmount.ToString("F2");
        }
    }
    public void UndoRGivingAmount()
    {
        GivingAmount = 0;
        GivingAmountText.text = GivingAmount.ToString("F2");
    }
    public void ConfirmGivingPayment()
    {
        if (GivingAmount < Change)
        {
            Debug.LogError("Giving Mmoney Is Less Than Change");
        }
        else if(ReceivedAmount!=0)
        {
            Debug.LogError("Giving Mmoney  is paid");
            CashDrawer.DrawerClose();
            if (ConfirmPaymentButton != null)
            {
                ConfirmPaymentButton.gameObject.SetActive(false);
            }
            Player playerScript = FindAnyObjectByType<Player>();
            if (playerScript)
            {
                playerScript.ConfirmPaymentFunc();
            }
            ReceivedAmountText.text = "$ 0.00";
            TotalAmount.text = "$ 0.00";
            ChangeText.text = "$ 0.00";
            GivingAmountText.text = "$ 0.00";
            ReceivedAmount = 0;
            Change = 0;
            GivingAmount = 0;
            CashRegisterCanvas.SetActive(false);
            CustomerBillCanvas.SetActive(true);
        }
        
    }

}
