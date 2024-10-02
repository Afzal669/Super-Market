using System.Collections;
using System.Collections.Generic;
using System.IO; // Make sure to include this
using System.Linq;
using UnityEngine;

public class ShelfParent : MonoBehaviour
{
  public  List<ShelfPlacement> shelfSlots = new List<ShelfPlacement>();

    private void Awake()
    {
        shelfSlots = GetComponentsInChildren<ShelfPlacement>().ToList();
    }




    public void LoadShelf()
    {
        Invoke(nameof(enableTrigger), 1.5f);

    }
    void enableTrigger()
    {

        foreach (ShelfPlacement slot in shelfSlots)
        {
            slot.InTrigger = true;
        }
        Invoke(nameof(disableTrigger), 2f);
    }
    void disableTrigger()
    {

        foreach (ShelfPlacement slot in shelfSlots)
        {
            slot.InTrigger = false;
        }
    }
}

[System.Serializable]
public class ShelfDataWrapper
{
    public List<ShelfSlotData> shelfData;

    public ShelfDataWrapper(List<ShelfSlotData> shelfData)
    {
        this.shelfData = shelfData;
    }
}

[System.Serializable]
public class ShelfSlotData
{
    public string shelfplaceName;
    public string CurrentProductName;
    public int Quantity;

    public ShelfSlotData(string shelfp, string currentProduct, int quantity)
    {
        this.shelfplaceName = shelfp;
        this.CurrentProductName = currentProduct;
        this.Quantity = quantity;
    }
}
