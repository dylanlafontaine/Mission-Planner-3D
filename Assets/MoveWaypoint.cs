using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveWaypoint : MonoBehaviour
{
    public Button myButton;
    private Waypoints waypoints;

    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    private void TaskOnClick()
    {

        waypoints.setMoveFlag(SpawnUI.selectedWaypoint);
    }
}
