using UnityEngine;

public class AddRemoveRigidbody : MonoBehaviour
{
    private Rigidbody rb;
    public bool isPacementObject;
    void Start()
    {
        // Add Rigidbody component to the GameObject
        if (gameObject.GetComponent<Rigidbody>())
        {
            rb = null;
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
           // rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        }

    }

    void OnCollisionEnter(Collision collision)
    {
  
          //  Invoke(nameof(RemoveRbAndScript), 4f);
    }
    

    void RemoveRbAndScript()
    {
        Destroy(rb);
        Destroy(this);
    }
}