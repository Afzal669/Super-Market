using System.Collections.Generic;
using UnityEngine;

public class BoxAddRemove : MonoBehaviour
{
    public List<Transform> BoxList = new List<Transform>();
    public GameObject Product;
    public bool isOpenBox;
    [SerializeField] BoxLid Boxdoor;
    [SerializeField] BoxLid Boxdoor1;
    public void AddProduct(GameObject product)
    {
        if (BoxList.Count <= 12)
        {
            Product = product;
            BoxList.Add(product.transform);
        }
      
    }
     public bool IscurrentProduct(string Itemname)
    {
        if(Product!=null)
        {
            if(Product.GetComponent<item>().Name==Itemname)
            {
                return true;

            }
        }
        return false;
    }
    
    public GameObject RemoveProduct()
    {
        if(BoxList.Count>0)
        {
            if (BoxList.Count > 0)
            {
                GameObject o = BoxList[BoxList.Count - 1].gameObject;
                o.transform.SetParent(null);
                print(o.transform.localScale);
                BoxList.RemoveAt(BoxList.Count - 1);
                return o;
            }
        }
        return null;

    }



    public void boxopen()
    {
        isOpenBox = true;
        Boxdoor.OpenLid();
        Boxdoor1.OpenLid();
    }

 


}
