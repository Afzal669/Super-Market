
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShelfPlacement : MonoBehaviour
{
    public GameObject cartonBox;                       
    float spacingFactorZ = 0.1f; 
    public GameObject currentProduct;
    private Vector3 productSize;                   
    private float spacingX;             
    public int countZ;
    public bool IsBoxPlane;
    float forwardSideLength;
  

    // List of ShelfSlot objects to manage products and their positions
    public List<ShelfSlot> shelfSlots = new List<ShelfSlot>();
    void SetupPlacement(GameObject product)
    {
        currentProduct = product;
        BoxCollider productRenderer = product.GetComponent<BoxCollider>();
        productSize = productRenderer.bounds.size;
        spacingX = productSize.x * spacingFactorZ;
        BoxCollider boxRenderer = cartonBox.GetComponent<BoxCollider>();
        Vector3 BoxSize = boxRenderer.bounds.size;
        float forwardSideLength = Mathf.Max(Mathf.Abs(BoxSize.x), Mathf.Abs(BoxSize.y), Mathf.Abs(BoxSize.z));
        countZ = Mathf.FloorToInt((forwardSideLength) / (productSize.x + spacingX));
        print(productSize.x);
       
    }

    void InitializeSlots()
    {
        for (int i = 0; i < countZ; i++)
        {
            int z = i % countZ; 

            Vector3 slotPosition = cartonBox.transform.position;
            Vector3 boxForward = cartonBox.transform.forward;

            // Calculate the offset from the front of the box
            float zOffset = z * (productSize.x + spacingX) - forwardSideLength / 2 + productSize.x / 2;
        
            // Adjust the slot position based on the box's forward direction

            slotPosition += boxForward * zOffset;
            Vector3 localPosition = cartonBox.transform.InverseTransformPoint(slotPosition);

            // Create a new ShelfSlot and add it to the list
            shelfSlots.Add(new ShelfSlot(localPosition));
        }
    }
    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveRandomProduct();
        }
    }




    public bool hasAvailableSlot()
    {
        return shelfSlots.Exists(slot => slot.isPlaceable);
    }
    public void AddProduct(GameObject product)
    {
      
        // If this is the first product, initialize the slots
        if (shelfSlots.Count == 0)
        {
            SetupPlacement(product);
            InitializeSlots();
        }

        // Try to place the product in the first available slot
        foreach (ShelfSlot slot in shelfSlots)
        {
            if (slot.isPlaceable)
            {
                product.transform.SetParent(transform);
                PlaceProduct(slot, product);
                break;
            }
        }
    }
    public void RemoveRandomProduct()
    {
        List<ShelfSlot> occupiedSlots = shelfSlots.FindAll(slot => !slot.isPlaceable);

        if (occupiedSlots.Count == 0) return; 
        ShelfSlot randomSlot = occupiedSlots[Random.Range(0, occupiedSlots.Count)];
        Destroy(randomSlot.product);
        randomSlot.isPlaceable = true;
        randomSlot.product = null;
        foreach(ShelfSlot s in shelfSlots)
        {
            if (!s.isPlaceable)
                return;
        }
        currentProduct = null;
        shelfSlots.Clear();
        countZ = 0;

    }
    private void PlaceProduct(ShelfSlot slot, GameObject product)
    {
        if (!slot.isPlaceable) return;

        // Use cartonBox rotation to align product correctly
        StartCoroutine(MoveInParabola(product.transform, slot.position, 0.5f,1));
      //  product.transform.localPosition = slot.position;
        product.transform.rotation = cartonBox.transform.rotation * Quaternion.Euler(0, 90, 0); // Adjust this rotation as needed
        slot.product = product;
        slot.isPlaceable = false;
    }
     IEnumerator MoveInParabola(Transform objectToMove, Vector3 targetPosition, float duration, float arcHeight)
    {
        Vector3 startPosition = objectToMove.localPosition; // Initial position
        float timeElapsed = 0f;


        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration; // Normalize time to [0,1]

            // Linear interpolation between start and target positions
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Adding the parabolic height to the Y-axis using a sine function for smoother motion
            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // Set the current position with the height offset
            currentPosition.y += heightOffset;

            // Apply the new position
            objectToMove.localPosition= currentPosition;

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the target position at the end
        objectToMove.localPosition = targetPosition;
    }

   


}
[System.Serializable]
public class ShelfSlot
{
    public GameObject product; 
    public Vector3 position;  
    public bool isPlaceable;   

    public ShelfSlot(Vector3 position)
    {
        this.position = position;
        this.isPlaceable = true; 
        this.product = null;     
    }
}


