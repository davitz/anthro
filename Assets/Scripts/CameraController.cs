using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Zoom Properties (Levels: 0 is close, 15 is far)")]
    public float ZoomSpeed = 10.0f;
    public float MinimumZoomLevel = 0f;
    public float MaximumZoomLevel = 15f;
    private float ZoomLevel = 0f;

    void Update()
    {
        // Negative axis: zoom out, positive: zoom in
        float axis = Input.GetAxis("Mouse ScrollWheel");

        // This is a really shitty temporary fix for camera tunnelling through player
        // Still happens but only if you scroll really fast
        if (ZoomLevel < MinimumZoomLevel) ZoomLevel = MinimumZoomLevel;
        if (ZoomLevel > MaximumZoomLevel) ZoomLevel = MaximumZoomLevel;

        if (axis < 0) // zoom out
        {
            // if level is above minimum and under maximum
            if (ZoomLevel >= MinimumZoomLevel && ZoomLevel < MaximumZoomLevel)
            {
                this.transform.Translate((-this.transform.forward * -axis) * ZoomSpeed, Space.World);
                ZoomLevel += (-axis);
            }
        }
        else if (axis > 0) // zoom in
        {
            if (ZoomLevel > MinimumZoomLevel && ZoomLevel <= MaximumZoomLevel)
            {
                this.transform.Translate((-this.transform.forward * -axis) * ZoomSpeed, Space.World);
                ZoomLevel += (-axis);
            }
        }
    }
}
