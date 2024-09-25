using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class PlayerDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] splineMove vehicle;
    [SerializeField] WheelRotation[] wheels;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")  || other.CompareTag("TrafficCar"))
        {
            //print("__Player Detected");
            vehicle.Pause();
            for (int i = 0; i < wheels.Length; i++)
                wheels[i].stopRotation = true;
        }
       

//        print("__Player not  Detected_"+other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("TrafficCar"))
        {
            //print("__Player Detected");
            if(!vehicle.IsPaused())
                vehicle.Pause();
            for (int i = 0; i < wheels.Length; i++)
                wheels[i].stopRotation = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("TrafficCar"))
        {
            Invoke("ResumeVehicle", 2);
        }
    }


    void ResumeVehicle()
    {
        vehicle.Resume();
        for (int i = 0; i < wheels.Length; i++)
            wheels[i].stopRotation = false;
    }
}
