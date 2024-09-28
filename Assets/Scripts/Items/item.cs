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
    public int Space = 0;
    public Type Display;


    void Start()
    {
        Self = transform.gameObject;
      //  Name = transform.name;
   
    }

}
