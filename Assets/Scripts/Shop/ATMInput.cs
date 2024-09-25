using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATMInput : MonoBehaviour
{
    public TextMeshProUGUI displayText; // Reference to the TextMeshPro text where the result will be displayed
    private string inputText = "";
    private bool hasDecimalPoint = false;

    public void OnNumberButtonPress(string number)
    {
        if (number == ".")
        {
            if (!hasDecimalPoint) // Allow only one decimal point
            {
                inputText += number;
                hasDecimalPoint = true; // Mark that a decimal point has been used
                UpdateDisplay();
            }
        }
        else if (inputText.Length < 10) // Limit input length to 10 characters
        {
            // Append the number and ensure leading zeros are removed
            if (inputText == "0" && number != ".")
            {
                inputText = number; // Replace leading zero if not followed by a decimal point
            }
            else
            {
                inputText += number;
            }
            UpdateDisplay();
        }
    }

    public void OnClearButtonPress()
    {
        inputText = "";
        hasDecimalPoint = false;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Check if there's valid input to display
        if (inputText.Length > 0)
        {
            // Handle cases where there's a leading zero
            if (inputText.StartsWith("0") && !inputText.StartsWith("0.") && inputText.Length > 1)
            {
                inputText = inputText.TrimStart('0'); // Remove leading zeros
            }

            displayText.text = "$" + inputText;
        }
        else
        {
            displayText.text = "$0.00"; // Default display when no input
        }
    }
}
