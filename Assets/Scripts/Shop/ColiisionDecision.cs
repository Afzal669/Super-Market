using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiisionDecision : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isColliding = true;
    public bool isOnStorePlace = false;
    private void OnCollisionStay(Collision collision)
    {
        Vector3 _hit = collision.GetContact(0).normal;
        float angel = Vector3.Angle(_hit, transform.up);
        //print(collision.transform.name);
        if(Mathf.Approximately(angel,0))
        {
           // print("Coliiding Down");
            //isColliding = false;
        }
        else if(Mathf.Approximately(angel,180))
        {
            isColliding = true;
          //  print("Up");
        }
        else if(Mathf.Approximately(angel,90))
        {
            isColliding = true;
            Vector3 cross = Vector3.Cross(Vector3.forward, _hit);
            if(cross.y > 0)
            {
              //  print("Left");
            }
            else
            {
               // print("Rigt");
            }
        }
        else
        {
           // isColliding = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(isOnStorePlace)
        {
            isColliding = false;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "StorePlace")
        {
            isOnStorePlace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "StorePlace")
        {
            isOnStorePlace = false;
        }
    }
}
