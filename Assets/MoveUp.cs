using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveUp : MonoBehaviour
{
    public Button up, down;
    private Waypoints waypoints;
    public GameObject selectedSphere;
    public Waypoint selectedWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        up.onClick.AddListener(moveUp);
        down.onClick.AddListener(moveDown);
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void moveUp()
    {
        waypoints.moveWaypointUp(spawnUI.selectedWaypoint);
    }

    void moveDown()
    {
        waypoints.moveWaypointDown(spawnUI.selectedWaypoint);
    }
}
