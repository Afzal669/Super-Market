using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenrateBox : MonoBehaviour
{
     public GameObject Box;
     public Transform BoxPostion;
     int productsPerRowX = 2;     
     int productsPerRowZ = 3;      
     float spacingFactorX = 0.05f; 
     float spacingFactorY = 0.05f;
     float spacingFactorZ = 0.05f;  
     float boxSizeOffset = 0.05f;   
    public static GenrateBox Instance;
    private void Awake()
    {
        Instance = this;
    }

    public  void CreateBox(GameObject Product, int quantity)
    {
        GameObject box = Instantiate(Box, transform.position, Quaternion.identity);
        List<GameObject> list = new List<GameObject>();
        BoxCollider productCollider = Product.GetComponent<BoxCollider>();
        BoxAddRemove boxScript = box.GetComponent<BoxAddRemove>();
        boxScript.Product = Product;
        Vector3 productSize = productCollider.size;
        Vector3 spacing = new Vector3(
            productSize.x * spacingFactorX,
            productSize.y * spacingFactorY,
            productSize.z * spacingFactorZ
        );

    
        float totalProductWidth = (productsPerRowX * productSize.x) + (productsPerRowX - 1) * spacing.x;
        float totalProductDepth = (productsPerRowZ * productSize.z) + (productsPerRowZ - 1) * spacing.z;
        int productsPerLayer = productsPerRowX * productsPerRowZ; 
        int totalLayers = Mathf.CeilToInt(quantity / (float)productsPerLayer); 
        float totalProductHeight = (totalLayers * productSize.y) + (totalLayers - 1) * spacing.y;
        Renderer boxRenderer = box.GetComponent<Renderer>();
        Bounds boxBounds = boxRenderer.bounds;
        Vector3 newBoxSize = new Vector3(
            totalProductWidth,
            totalProductHeight,
            totalProductDepth
        );
        AdjustBoxBounds(boxRenderer, newBoxSize,box);
        Vector3 boxCenter = new Vector3(newBoxSize.x / 2, boxBounds.center.y, newBoxSize.z / 2);
        box.transform.position = boxCenter;
        int productCount = 0;
        for (int y = 0; y < totalLayers && productCount < quantity; y++) // Layer (Y-axis) loop
        {
            for (int x = 0; x < productsPerRowX && productCount < quantity; x++) // Row (X-axis) loop
            {
                for (int z = 0; z < productsPerRowZ && productCount < quantity; z++) // Column (Z-axis) loop
                {
                    // Calculate the position for each product relative to the box center
                    Vector3 productPosition;
                    if ( totalLayers==1)
                    {
                         productPosition = box.transform.position
                          + new Vector3(
                        x * (productSize.x + spacing.x) - newBoxSize.x / 2 + productSize.x / 2,
                        y * (productSize.y + spacing.y) - newBoxSize.y / 2 + productSize.y/2,
                        z * (productSize.z + spacing.z) - newBoxSize.z / 2 + productSize.z / 2
                         );
                    }
                    else
                    {
                        productPosition = box.transform.position
                       + new Vector3(
                       x * (productSize.x + spacing.x) - newBoxSize.x / 2 + productSize.x / 2,
                       y * (productSize.y + spacing.y) - newBoxSize.y / 2 + productSize.y,
                       z * (productSize.z + spacing.z) - newBoxSize.z / 2 + productSize.z / 2
                   );
                    }
               

               
                    GameObject O = Instantiate(Product, productPosition, Quaternion.identity);
                    list.Add(O);
                    boxScript.AddProduct(O);
                    productCount++;
                }
            }
        }
        newBoxSize = new Vector3(
       totalProductWidth + boxSizeOffset,
       totalProductHeight + boxSizeOffset,
       totalProductDepth + boxSizeOffset
);
        AdjustBoxBounds(boxRenderer, newBoxSize, box);
        foreach (GameObject obj in list)
        {
            obj.GetComponent<BoxCollider>().enabled = true;
            obj.transform.SetParent(box.transform);
        }
          box.transform.position = BoxPostion.position;
        box.AddComponent<AddRemoveRigidbody>();

    }
    void AdjustBoxBounds(Renderer boxRenderer, Vector3 newBoxSize,GameObject box)
    {
        Vector3 currentBoxSize = boxRenderer.bounds.size;
        Vector3 scaleFactor = new Vector3(
            newBoxSize.x / currentBoxSize.x,
            newBoxSize.y / currentBoxSize.y,
            newBoxSize.z / currentBoxSize.z
        );
        box.transform.localScale = Vector3.Scale(box.transform.localScale, scaleFactor);
      
    }


   

}
    
