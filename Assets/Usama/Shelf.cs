using UnityEngine;

public class Shelf : MonoBehaviour
{
    public Transform[] customerPositions;  // Array of positions where customers can stand
    private bool[] isPositionOccupied;     // Array to track occupied positions
   

    private void Start()
    {
        isPositionOccupied = new bool[customerPositions.Length];
    }

    // Checks if the shelf has available space
    public bool HasAvailableSpace()
    {
        foreach (bool occupied in isPositionOccupied)
        {
            if (!occupied)
            {
                return true; // Space available
            }
        }
        return false; // No space available
    }

    // Reserves an available position for the customer
    public Vector3 ReservePosition(out int reservedIndex)
    {
        for (int i = 0; i < isPositionOccupied.Length; i++)
        {
            if (!isPositionOccupied[i])
            {
                isPositionOccupied[i] = true; // Mark the position as occupied
                reservedIndex = i; // Return the index of the reserved position
                return customerPositions[i].position; // Return the position
            }
        }
        reservedIndex = -1; // No available position
        return Vector3.zero;
    }

    // Frees the specific position after the customer has finished
    public void FreePosition(int positionIndex)
    {
        if (positionIndex >= 0 && positionIndex < isPositionOccupied.Length)
        {
            isPositionOccupied[positionIndex] = false; // Free the specific position
        }
    }
}
