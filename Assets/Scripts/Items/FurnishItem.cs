using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnishItem : MonoBehaviour
{
    public string Name;
    public float Unit_Price = 0;
    public Collider offSetCollider,bigCollider,extraCol;
    public GameObject targetGreenMesh;
    public List<GameObject> childs;

    public bool isOnRightPlace = false;

    private void Start()
    {
        //Name = transform.name;

    }

   
}
