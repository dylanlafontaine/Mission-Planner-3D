using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLatitude : MonoBehaviour
{
    public Text waypointNum;
    private Waypoints waypoints;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

    public void HandleChangeLatitude(InputField latitudeInput) {
        double latitude, longitude;
        foreach (Waypoint waypoint in waypoints.points) {
            if (waypoint.Number == int.Parse(waypointNum.text)) {
                waypoint.Marker.GetPosition(out longitude, out latitude);
                waypoint.Marker.SetPosition(longitude, double.Parse(latitudeInput.text));
                break;
            }
        }
    }
}
