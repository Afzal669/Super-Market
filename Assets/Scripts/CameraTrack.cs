using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    // Public variables to assign in the Inspector
    public Transform player; // The player's transform
    public Vector3 offset;   // Offset between the camera and player
    public float smoothSpeed = 0.125f; // Smoothness of camera movement

    void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = smoothedPosition;

        // Optionally, you can make the camera look at the player
        transform.LookAt(player);
    }

}
