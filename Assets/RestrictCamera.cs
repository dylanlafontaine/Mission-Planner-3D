using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Restricts the Camera's Pan and Tilt features if the zoom level of the Map is less than 6 to hide the edge of the map.
///Once the zoom level is greater than or equal to 6 the user is free to pan and tilt the camera.
///</summary>

public class RestrictCamera : MonoBehaviour
{
    private Vector2 defaultRotation;

    //Start is called once before the first frame is rendered
    void Start() {
        defaultRotation.x = 0;
        defaultRotation.y = 0;
    }

    // Update is called once per frame
    void Update() {
        int zoomValue = OnlineMaps.instance.zoom;
        
        if (zoomValue < 6) {
            OnlineMapsCameraOrbit.instance.lockPan = true;
            OnlineMapsCameraOrbit.instance.lockTilt = true;
            OnlineMapsCameraOrbit.instance.rotation = defaultRotation;
        }
        else {
            OnlineMapsCameraOrbit.instance.lockPan = false;
            OnlineMapsCameraOrbit.instance.lockTilt = false;
        }
    }
}
