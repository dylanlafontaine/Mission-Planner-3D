using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLongitude : MonoBehaviour
{
    public Text waypointNum;
    private Waypoints waypoints;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

    public void HandleChangeLongitude(InputField longitudeInput) {
        double latitude, longitude;
        foreach (Waypoint waypoint in waypoints.points) {
            if (waypoint.Number == int.Parse(waypointNum.text)) {
                waypoint.Marker.GetPosition(out longitude, out latitude);
                waypoint.Marker.SetPosition(double.Parse(longitudeInput.text), latitude);
                break;
            }
        }
    }
}
