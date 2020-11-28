using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAltitudeType : MonoBehaviour
{
    private Waypoints waypoints;

    void Start()
    {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

    public void HandleToggleAltitude(Toggle absoluteAltitudeToggle)
    {
        foreach (Waypoint waypoint in waypoints.points)
        {
            if (absoluteAltitudeToggle.isOn) {
                Debug.Log("True");
                waypoint.Marker.altitudeType = OnlineMapsAltitudeType.absolute;
            }
            else {
                Debug.Log("False");
                waypoint.Marker.altitudeType = OnlineMapsAltitudeType.relative;
            }
        }
        OnlineMaps.instance.Redraw();
    }
}
