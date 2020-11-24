using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
///<summary>
///Commented by Dylan L.
///MoveUp -- This class handles the initial behavior of moving a waypoint either up or down.
///It adds Event Handlers to the MoveUpList and MoveDownList buttons that changes the order in the Waypoints list.
///When the user clicks the up or down button then the function to handle the moving occurs in the waypoints class
///and the waypoint that was selected moves either up or down based on the button pressed. This change will be re-renderd and
///the directional "arrow" will be facing the correct way.
///</summary> 
public class MoveUp : MonoBehaviour
{
    //up represents the button labeled "Up" in the MoveUp script object
    //down represents the button labeled "Down" in the MoveUp script object
    public Button up, down;
    //waypoints represents a GameObject of class type "Waypoints"
    private Waypoints waypoints;
    //selectedSphere is not initially assigned to any value
    public GameObject selectedSphere;
    //selectedWaypoint is not initially assigned to any value
    public Waypoint selectedWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        //Adds Event Listeners to the "up" and "down" buttons that call the "moveUp" and "moveDown" functions respectively
        up.onClick.AddListener(moveUp);
        down.onClick.AddListener(moveDown);
        //waypoints is initialized to the Waypoints class
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    private void moveUp()
    {
        //calls the moveWaypointUp function defined in the Waypoints class
        waypoints.moveWaypointUp(SpawnUI.selectedWaypoint);
    }

    private void moveDown()
    {
        //calls the moveWaypointDown function defined in the Waypoints class
        waypoints.moveWaypointDown(SpawnUI.selectedWaypoint);
    }
}
