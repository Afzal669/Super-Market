using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashMachine : MonoBehaviour
{
    public List<Transform> customerPositions = new List<Transform>(); // Positions in front of the cash machine
    public List<Transform> Itemplacement = new List<Transform>(); // Item placement positions
    public List<CharcaterBehaviour> waitingCustomers = new List<CharcaterBehaviour>();
 

    public Transform GetAvailablePosition()
    {
        if (waitingCustomers.Count < customerPositions.Count)
        {  
            return customerPositions[waitingCustomers.Count];
        }
        return null;
    }

    public void MoveCharacterForward()
    {
        if (waitingCustomers.Count > 0)
        {
            waitingCustomers.RemoveAt(0); // Remove the first customer who completed payment

            for (int i = 0; i < waitingCustomers.Count; i++)
            {
                waitingCustomers[i].MoveToPosition(customerPositions[i],true); // Move remaining customers forward
            }
        }
    }

    public void AddCustomerToQueue(CharcaterBehaviour customer)
    {
        if (waitingCustomers.Count < customerPositions.Count)
        {
            waitingCustomers.Add(customer);
        }
        else
        {
            Debug.Log("No available position for new customer.");
        }
    }
}
