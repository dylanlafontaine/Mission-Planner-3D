using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWaypointRadius : MonoBehaviour
{
    private Waypoints waypoints;

    void Start()
    {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

    public void HandleChangeRadius(InputField radiusInput)
    {
        foreach (Waypoint waypoint in waypoints.points)
        {
            waypoint.Radius = int.Parse(radiusInput.text);
            waypoint.Marker.scale = 9 + int.Parse(radiusInput.text);
        }
    }
}
