using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveUp : MonoBehaviour
{
    public Button up, down;
    public List<Waypoint> points;
    public GameObject selectedSphere;
    public Waypoint selectedWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        up.onClick.AddListener(moveUp);
        down.onClick.AddListener(moveDown);
        points = ((Waypoints)FindObjectOfType(typeof(Waypoints))).points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void moveUp()
    {
        int index;
        Waypoint temp;
        selectedWaypoint = spawnUI.selectedWaypoint;
        index = points.IndexOf(selectedWaypoint);
        if (index < points.Count - 1)
        {
            temp = points[index];
            points[index] = points[index + 1];
            points[index + 1] = temp;
        }
    }

    void moveDown()
    {
        int index;
        Waypoint temp;
        selectedWaypoint = spawnUI.selectedWaypoint;
        index = points.IndexOf(selectedWaypoint);
        if (index > 0)
        {
            temp = points[index];
            points[index] = points[index - 1];
            points[index - 1] = temp;
        }
    }
}
