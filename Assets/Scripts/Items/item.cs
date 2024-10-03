using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public enum Type
    {
        Shelf,
        Fridge
    }
    public GameObject Self;
    public Sprite sprite;
    public string Name;
    public float Unit_Price = 0;
    public float Gross_Price = 0;
    public int Space = 0;
    public Type Display;
    public bool Reserved = false;

    void Start()
    {
        string updatedName = transform.name.Replace("(Clone)", "");
        Self = transform.gameObject;
        Gross_Price = Unit_Price;
        Name = updatedName;
   
    }

    private void OnDestroy()
    {
        ShelfPlacement shelfPlace = GetComponentInParent<ShelfPlacement>();
        if (shelfPlace)
        {
            shelfPlace.SetCurrentProduct(gameObject);
        }
    }

}
