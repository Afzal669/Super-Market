using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cash_Machine : MonoBehaviour
{
    [Header("Cash Machine")]
    public Transform positionOnChair;
    public Transform positionOffChair;
    public Collider IntractCollider;
    public Camera ATM_Camera;
    public Transform DetailingPanel;
    public TextMeshProUGUI BillAmount;
    public CashDrawerWorking cashDrawer;
    public Canvas GivingMoneyCanvas;
    public Canvas TotalBillCanvas;
    void Start()
    {
        StartCoroutine(AssignCameraAfterDelay(5f)); // Start the coroutine to check and assign the camera after a delay
    }
    IEnumerator AssignCameraAfterDelay(float delay)
    {
        // Wait for the given delay (e.g., 5 seconds)
        yield return new WaitForSeconds(delay);

        // Find the GameObject tagged as "MainCamera"
        GameObject FPS = GameObject.FindGameObjectWithTag("MainCamera");

        // If the MainCamera is found, assign it to the world-space canvases
        if (FPS)
        {
          //  Debug.Log("Main Camera found");

            // Assign the MainCamera to the GivingMoneyCanvas if it's in WorldSpace mode
            if (GivingMoneyCanvas.renderMode == RenderMode.WorldSpace)
            {
               // Debug.Log("Assigning GivingMoneyCanvas Event Camera");
                GivingMoneyCanvas.worldCamera = FPS.GetComponent<Camera>();
            }

            // Assign the MainCamera to the TotalBillCanvas if it's in WorldSpace mode
            if (TotalBillCanvas.renderMode == RenderMode.WorldSpace)
            {
                //Debug.Log("Assigning TotalBillCanvas Event Camera");
                TotalBillCanvas.worldCamera = FPS.GetComponent<Camera>();
            }
        }
        else
        {
           // Debug.LogWarning("Main Camera not found. Retrying in 5 seconds.");
            // Retry by restarting the coroutine
            StartCoroutine(AssignCameraAfterDelay(delay));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
