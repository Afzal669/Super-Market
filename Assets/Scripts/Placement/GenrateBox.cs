using System.Collections;
using System.Collections.Generic;
using System.IO;
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
   // public List<GameObject> AllItems = new List<GameObject>();
     List<BoxAddRemove> GenerateBoxList = new List<BoxAddRemove>();
    private void Awake()
    {
        Instance = this;
    }

    public void CreateBox(GameObject Product, int quantity)
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
        AdjustBoxBounds(boxRenderer, newBoxSize, box);
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
                    if (totalLayers == 1)
                    {
                        productPosition = box.transform.position
                         + new Vector3(
                       x * (productSize.x + spacing.x) - newBoxSize.x / 2 + productSize.x / 2,
                       y * (productSize.y + spacing.y) - newBoxSize.y / 2 + productSize.y / 2,
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
            // obj.GetComponent<BoxCollider>().enabled = false;
            obj.transform.SetParent(box.transform);
        }
        box.transform.position = BoxPostion.position;
        box.AddComponent<AddRemoveRigidbody>();
        GenerateBoxList.Add(boxScript);

    }
    public void CreateBox(GameObject Product, int quantity, Vector3 pos, Quaternion rot)
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
        AdjustBoxBounds(boxRenderer, newBoxSize, box);
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
                    if (totalLayers == 1)
                    {
                        productPosition = box.transform.position
                         + new Vector3(
                       x * (productSize.x + spacing.x) - newBoxSize.x / 2 + productSize.x / 2,
                       y * (productSize.y + spacing.y) - newBoxSize.y / 2 + productSize.y / 2,
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
            // obj.GetComponent<BoxCollider>().enabled = false;
            obj.transform.SetParent(box.transform);
        }
        box.transform.position = pos;
        box.transform.rotation = rot;
        box.AddComponent<AddRemoveRigidbody>();
        GenerateBoxList.Add(boxScript);

    }
    void AdjustBoxBounds(Renderer boxRenderer, Vector3 newBoxSize, GameObject box)
    {
        Vector3 currentBoxSize = boxRenderer.bounds.size;
        Vector3 scaleFactor = new Vector3(
            newBoxSize.x / currentBoxSize.x,
            newBoxSize.y / currentBoxSize.y,
            newBoxSize.z / currentBoxSize.z
        );
        box.transform.localScale = Vector3.Scale(box.transform.localScale, scaleFactor);

    }
    public GameObject GetProductPrefab(string productname)
    {
        foreach (GameObject o in SavingSystem.Instance.prefabList)
        {
            if (o.name == productname)
                return o;
        }
        return null;
    }

    /*  
        public void SaveBoxState()
        {
            BoxDataWrapper dataWrapper = new BoxDataWrapper();

            // Create data objects for each product in the box
            foreach (BoxAddRemove product in GenerateBoxList)
            {
                if (product != null)
                {
                    BoxSaveData data = new BoxSaveData(
                        product.name,
                        product.transform.position,
                        product.transform.rotation,
                        product.Product.GetComponent<item>().Name,
                        product.BoxList.Count
                    );
                    dataWrapper.productDataList.Add(data);
                }
            }

            // Convert to JSON and save to a file
            string json = JsonUtility.ToJson(dataWrapper);
            string path = Application.persistentDataPath + "/boxsave.json";
            File.WriteAllText(path, json);

            Debug.Log("Box state saved to " + path);
        }



        public void LoadBoxState()
        {
            string path = Application.persistentDataPath + "/boxsave.json";

            if (File.Exists(path))
            {
                // Load the JSON file
                string json = File.ReadAllText(path);

                // Deserialize JSON into a BoxDataWrapper object
                BoxDataWrapper loadedData = JsonUtility.FromJson<BoxDataWrapper>(json);

                GenerateBoxList.Clear();

                // Recreate the boxes and products
                foreach (BoxSaveData data in loadedData.productDataList)
                {
                    if(data.productCount>0)
                    CreateBox(GetProductPrefab(data.ProductName), data.productCount, data.boxPosition, data.boxRotation);
                }

                Debug.Log("Box state loaded from " + path);
            }
            else
            {
                Debug.LogWarning("No save file found at " + path);
            }
        }*/

    public void SaveBoxState()
    {
        // Clear previous box data
        PlayerPrefs.SetString("BoxData", "");
       

        // Create data objects for each product in the box
        foreach (BoxAddRemove product in GenerateBoxList)
        {
            if (product != null&&product.BoxList.Count>0)
            {
                string data = $"{product.name},{product.transform.position.x},{product.transform.position.y},{product.transform.position.z}," +
                              $"{product.transform.rotation.x},{product.transform.rotation.y},{product.transform.rotation.z},{product.transform.rotation.w}," +
                              $"{product.Product.GetComponent<item>().Name},{product.BoxList.Count}";

                // Append data to PlayerPrefs
                string existingData = PlayerPrefs.GetString("BoxData");
                PlayerPrefs.SetString("BoxData", existingData + data + "|");
            }
        }

        PlayerPrefs.Save();
        Debug.Log("Box state saved to PlayerPrefs.");
    }

    public void LoadBoxState()
    {
        string data = PlayerPrefs.GetString("BoxData");

        if (!string.IsNullOrEmpty(data))
        {
            // Split the data by '|'
            string[] boxesData = data.Split('|');

            GenerateBoxList.Clear();

            // Recreate the boxes and products
            foreach (string boxData in boxesData)
            {
                if (!string.IsNullOrEmpty(boxData))
                {
                    string[] values = boxData.Split(',');

                    if (values.Length >= 8)
                    {
                        string boxName = values[0];
                        Vector3 boxPosition = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                        Quaternion boxRotation = new Quaternion(float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]), float.Parse(values[7]));
                        string productName = values[8];
                        int productCount = int.Parse(values[9]);

                        CreateBox(GetProductPrefab(productName), productCount, boxPosition, boxRotation);
                    }
                }
            }

            Debug.Log("Box state loaded from PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning("No save data found in PlayerPrefs.");
        }
    }





}





[System.Serializable]
public class BoxSaveData
{
    public string boxName;
    public Vector3 boxPosition;
    public Quaternion boxRotation;
    public string ProductName;
    public int productCount;


    public BoxSaveData(string bName, Vector3 boxPosition, Quaternion boxRotation,string ProductName, int Count)
    {
        this.boxName = bName;
        this.boxPosition = boxPosition;
        this.boxRotation = boxRotation;
        this.ProductName = ProductName;
        this.productCount = Count; // Automatically set the product count
    }
}

// Wrapper class for serialization
[System.Serializable]
public class BoxDataWrapper
{
    public List<BoxSaveData> productDataList = new List<BoxSaveData>();
}
