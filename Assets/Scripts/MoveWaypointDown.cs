using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveWaypointDown : MonoBehaviour
{
    public Text waypointNum;
    private ContentListDisplay display;
    private Waypoints waypoints;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        display = (ContentListDisplay)FindObjectOfType<ContentListDisplay>();
    }

    public void HandleMoveDown() {
        foreach (Waypoint waypoint in waypoints.points) {
            if (waypoint.Number == int.Parse(waypointNum.text)) {
                waypoints.moveWaypointUp(waypoint);
                break;
            }
        }
        OnlineMaps.instance.Redraw();
        display.Prime(waypoints.points);
    }
}
