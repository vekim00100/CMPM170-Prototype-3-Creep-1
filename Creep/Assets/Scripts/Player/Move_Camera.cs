using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Camera : MonoBehaviour
{
    public Transform cameraPosition;
    // Optional smoothing factor (higher = snappier). Set to 0 for no smoothing.
    public float followSmooth = 10f;

    // Use LateUpdate so the camera follows after all character movement/physics updates
    void LateUpdate()
    {
        if (followSmooth <= 0f)
        {
            transform.position = cameraPosition.position;
            return;
        }

        // Smoothly interpolate camera position to reduce jitter and create smooth motion
        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, followSmooth * Time.deltaTime);
    }
}
