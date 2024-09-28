using System.Collections;
using UnityEngine;

public class BoxLid : MonoBehaviour
{
    [SerializeField] Vector3 OpenRotation;   // The rotation when the box lid is fully open (e.g., 90 degrees on the X-axis)
    [SerializeField] Vector3 ClosedRotation; // The initial closed rotation (e.g., 0 degrees on the X-axis)
    private Quaternion openRotation;
    private Quaternion closedRotation;
    [SerializeField] float rotationSpeed = 2f; // Speed at which the lid rotates
    private Coroutine moveLidCoroutine;
    private bool isOpening = false;

    private void Start()
    {
        // Define the open and closed rotations using the Euler angles
        openRotation = Quaternion.Euler(OpenRotation);
        closedRotation = Quaternion.Euler(ClosedRotation);
    }

    // Method to open the box lid
    public void OpenLid()
    {
        if (!isOpening)
        {
            if (moveLidCoroutine != null)
            {
                StopCoroutine(moveLidCoroutine);
            }
            moveLidCoroutine = StartCoroutine(MoveLid(openRotation));  // Open the lid to the specified open rotation
        }
    }

    // Coroutine that moves the box lid to the open position
    IEnumerator MoveLid(Quaternion target)
    {
        isOpening = true;

        while (Quaternion.Angle(transform.localRotation, target) > 0.01f)
        {
            // Rotate the lid towards the target open position
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        // Ensure the lid snaps to the exact target rotation at the end
        transform.localRotation = target;
        isOpening = false;  // Mark that the lid is done opening
    }
}
