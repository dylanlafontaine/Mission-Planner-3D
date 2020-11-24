using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteWaypoint : MonoBehaviour
{
    public Button myButton;
    private Waypoints waypoints;

    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    // Update is called once per frame

    private void TaskOnClick()
    {
        waypoints.deleteWaypoint(SpawnUI.selectedWaypoint);
    }
}