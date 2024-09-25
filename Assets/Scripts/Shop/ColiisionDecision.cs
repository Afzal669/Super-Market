using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiisionDecision : MonoBehaviour
{
    public bool isColliding;

    private void OnCollisionStay(Collision collision)
    {
        Vector3 _hit = collision.GetContact(0).normal;

        float angel = Vector3.Angle(_hit, transform.up);

        if (Mathf.Approximately(angel, 0))
        {
            print("Coliiding Down");
            //isColliding = false;
        }
        else if (Mathf.Approximately(angel, 180))
        {
            isColliding = true;
            print("Up");
        }
        else if (Mathf.Approximately(angel, 90))
        {
            isColliding = true;
            Vector3 cross = Vector3.Cross(Vector3.forward, _hit);
            if (cross.y > 0)
            {
                print("Left");
            }
            else
            {
                print("Rigt");
            }
        }
        else
        {
            // isColliding = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}