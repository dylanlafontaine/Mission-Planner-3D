using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDeleteWaypoint : MonoBehaviour
{
    public Text waypointNum;
    private Waypoints waypoints;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

    public void HandleDeleteWaypoint() {
        foreach (Waypoint waypoint in waypoints.points) {
            if (waypoint.Number == int.Parse(waypointNum.text)) {
                waypoints.deleteWaypoint(waypoint);
            }
        }
    }
}
