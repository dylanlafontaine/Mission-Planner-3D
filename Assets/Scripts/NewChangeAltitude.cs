using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewChangeAltitude : MonoBehaviour
{
    public Text waypointNum;
    private Waypoints waypoints;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

    public void ChangeAltitude(InputField altitudeInput) {
        foreach (Waypoint waypoint in waypoints.points) {
            if (waypoint.Number == int.Parse(waypointNum.text)) {
                waypoint.Marker.altitude = int.Parse(altitudeInput.text);
                break;
            }
        }
        OnlineMaps.instance.Redraw();
    }
}
