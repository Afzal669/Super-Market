
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShelfPlacement : MonoBehaviour
{
    public GameObject cartonBox;
    float spacingFactorZ = 0.1f;
    public GameObject currentProduct;
    private Vector3 productSize;
    public float spacingX;
    public int countZ;
    float forwardSideLength;
    public bool InTrigger;
    public float productzsizeX;
    public bool isStartingAdd;
    public GameObject Tag;
    public float GrossPrice = 0;
    

    // List of ShelfSlot objects to manage products and their positions
    public List<ShelfSlot> shelfSlots = new List<ShelfSlot>();
 
    void SetupPlacement(GameObject product)
    {
        currentProduct = product;
        GrossPrice = currentProduct.GetComponent<item>().Gross_Price;
        product.transform.rotation = Quaternion.identity;
        BoxCollider productRenderer = product.GetComponent<BoxCollider>();
        productSize = productRenderer.bounds.size;
        productzsizeX = productSize.x;
        spacingX = productSize.z * spacingFactorZ;
       
        BoxCollider boxRenderer = cartonBox.GetComponent<BoxCollider>();
        Vector3 BoxSize = boxRenderer.bounds.size;
        float forwardSideLength = Mathf.Max(Mathf.Abs(BoxSize.x), Mathf.Abs(BoxSize.y), Mathf.Abs(BoxSize.z));
        countZ = Mathf.FloorToInt((forwardSideLength) / (productSize.x + spacingX));

    } 
    void SetupPlacement(GameObject product,bool issaving)
    {
        currentProduct = product;

        if (currentProduct)
        {
          //  print("working");
            Tag.SetActive(true);
            Tag.transform.GetChild(0).GetComponent<Tag>().price.text = "$" + GrossPrice.ToString();
            Tag.transform.GetChild(0).GetComponent<Tag>().image.sprite = currentProduct.GetComponent<item>().sprite;

        }

        
       // product.transform.rotation = Quaternion.identity;
       // BoxCollider productRenderer = product.GetComponent<BoxCollider>();
       // productSize = productRenderer.bounds.size;
      //  productzsizeX = productSize.x;
        spacingX = productzsizeX * spacingFactorZ;
       
        BoxCollider boxRenderer = cartonBox.GetComponent<BoxCollider>();
        Vector3 BoxSize = boxRenderer.bounds.size;
        float forwardSideLength = Mathf.Max(Mathf.Abs(BoxSize.x), Mathf.Abs(BoxSize.y), Mathf.Abs(BoxSize.z));
        countZ = Mathf.FloorToInt((forwardSideLength) / (productzsizeX + spacingX));

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
    void InitializeSlots(bool issaving)
    {
        for (int i = 0; i < countZ; i++)
        {
            int z = i % countZ;

            Vector3 slotPosition = cartonBox.transform.position;
            Vector3 boxForward = cartonBox.transform.forward;

            // Calculate the offset from the front of the box
            float zOffset = z * (productzsizeX + spacingX) - forwardSideLength / 2 + productzsizeX / 2;

            // Adjust the slot position based on the box's forward direction

            slotPosition += boxForward * zOffset;
            Vector3 localPosition = cartonBox.transform.InverseTransformPoint(slotPosition);

            // Create a new ShelfSlot and add it to the list
            shelfSlots.Add(new ShelfSlot(localPosition));
        }
    }
   /* private void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveRandomProduct();
        }
    }*/

    public void SetGrossValue(float val)
    {
        foreach(var v in shelfSlots)
        {
            if(v.product != null)
            {
                v.product.GetComponent<item>().Gross_Price = val;
            }
        }
    }
    public bool hasAvailableSlot()
    {
        RemoveProduct();
        return shelfSlots.Exists(slot => slot.isPlaceable);
    }
    public void AddProduct(GameObject product )
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
               product.GetComponent<item>().Gross_Price = GrossPrice;
               product.transform.SetParent(transform);
               PlaceProduct(slot, product);
               break;
                
              
            }
        }
    }
    public void AddProduct(GameObject product ,bool issaving)
    {
        
        // If this is the first product, initialize the slots
        if (shelfSlots.Count == 0)
        {
            if(transform.name == "Box (4)")
            {
                print("1");
            }
            
            SetupPlacement(product,true);
            InitializeSlots(true);
        }

        // Try to place the product in the first available slot
        foreach (ShelfSlot slot in shelfSlots)
        {
            if (slot.isPlaceable)
            {
                product.GetComponent<item>().Gross_Price = GrossPrice;
                product.transform.SetParent(transform);
                PlaceProduct(slot, product,issaving);
                break;
            }
        }
    }

    public void RemoveProduct()
    {

        ShelfSlot emptySlot = shelfSlots.Find(slot => slot.product == null);
        if (emptySlot != null)
        {
            emptySlot.isPlaceable = true;
            return;
        }
        else
        {
            return;
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
        foreach (ShelfSlot s in shelfSlots)
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
        product.GetComponent<BoxCollider>().enabled=false;
        // Use cartonBox rotation to align product correctly
        StartCoroutine(MoveInParabola(product.transform, slot.position, 0.5f, 1));
        //  product.transform.localPosition = slot.position;
        product.transform.rotation = cartonBox.transform.rotation * Quaternion.Euler(0, 90, 0); // Adjust this rotation as needed
        slot.product = product;
        slot.isPlaceable = false;
       if(!SavingSystem.Instance.IntanSavingList.Contains(product))
           SavingSystem.Instance.IntanSavingList.Add(product);
    }
    void PlaceProduct(ShelfSlot slot, GameObject product,bool issaving)
    {
      
        if (!slot.isPlaceable) return;

        // Use cartonBox rotation to align product correctly
       // StartCoroutine(MoveInParabola(product.transform, slot.position, 0.5f, 1));
          product.transform.localPosition = slot.position;
        product.transform.rotation = cartonBox.transform.rotation * Quaternion.Euler(0, 90, 0); // Adjust this rotation as needed
        slot.product = product;
        slot.isPlaceable = false;
       if(!SavingSystem.Instance.IntanSavingList.Contains(product))
           SavingSystem.Instance.IntanSavingList.Add(product);
        product.GetComponent<BoxCollider>().enabled = false;
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
            objectToMove.localPosition = currentPosition;

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the target position at the end
        objectToMove.localPosition = targetPosition;
    }

    public int productQuantity()
    {
        int i = 0;
        foreach (ShelfSlot s in shelfSlots)
        {
            if (!s.isPlaceable)
                i++;
        }
        return i;
    }

 
    
    private void OnTriggerStay(Collider other)
    {
        if (InTrigger)
        {
           
            if (other.CompareTag("Item"))
            {
                foreach (ShelfSlot s in shelfSlots)
                {
                    if (s.product == other.gameObject)
                        return;
                }
                if (transform.name == "Box (4)")
                {
                    print("istrigger______"); ;
                }
                AddProduct(other.gameObject,true);


            }
        }
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







