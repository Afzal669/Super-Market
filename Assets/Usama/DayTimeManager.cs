using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayTimeManager : MonoBehaviour
{
    public Text timeText; // Reference to UI Text to display the time
    private float currentTime; // Current time in minutes since 8 AM
    private const int DayDuration = 780; // Total minutes from 8 AM to 9 PM (13 hours)
    private bool isSleeping = false; // Control whether time progresses
    public bool open = false; // Control if the shop is open
    public TextMeshPro Shoptext;
    private void Start()
    {
        currentTime = 8 * 60 * 60; // Start at 7 AM
        UpdateTimeDisplay();
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            if (open)
            {
                currentTime += Time.deltaTime * (75600 / DayDuration); // 86400 seconds in a real day
                if (currentTime >= 75600)
                {
                    StartNewDay();
                }

                UpdateTimeDisplay();
            }

            yield return null;
        }
    }
    private void UpdateTimeDisplay()
    {
        int hours = Mathf.FloorToInt(currentTime / 3600f);
        int minutes = Mathf.FloorToInt((currentTime % 3600f) / 60f);

        string timePeriod = (hours >= 12) ? "PM" : "AM";
        if (hours > 12) hours -= 12;
        if (hours == 0) hours = 12;

        timeText.text = string.Format("{0:00}:{1:00} {2}", hours, minutes, timePeriod);
    }
    // Example function to manually trigger the new day (could be a button or event)
    public void StartNewDay()
    {
        // Reset time to 8:00 AM for the new day
       
        currentTime = 21*60*60; 
        open = false; // Close the shop until the user opens it again
        Shoptext.color = Color.red;
        Shoptext.text = "Close";
        SetOpenState(false);
    }

    // Example function to toggle the 'open' state
    public void SetOpenState(bool state)
    {
        open = state;
    }
}
