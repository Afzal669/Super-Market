using System.Collections.Generic;
using UnityEngine;

public class BoxAddRemove : MonoBehaviour
{
    public List<Transform> BoxList = new List<Transform>();
    public GameObject Product;
    [SerializeField] BoxLid Boxdoor;
    [SerializeField] BoxLid Boxdoor1;
    public void AddProduct(GameObject product)
    {
      if(BoxList.Count<=12)
        BoxList.Add(product.transform);
      
    }

    
    public GameObject RemoveProduct()
    {
        if(BoxList.Count>0)
        {
            if (BoxList.Count > 0)
            {
                GameObject o = BoxList[BoxList.Count - 1].gameObject;
                o.transform.SetParent(null);
                BoxList.RemoveAt(BoxList.Count - 1);
                return o;
            }
        }
        return null;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            boxopen();
    }

    public void boxopen()
    {
         Boxdoor.OpenLid();
        Boxdoor1.OpenLid();
    }


}
