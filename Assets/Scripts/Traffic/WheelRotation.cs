using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    // Start is called before the first frame update



    public float speed;

    public bool stopRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopRotation)
            transform.Rotate(Vector3.right, speed * Time.deltaTime);
    }
}
