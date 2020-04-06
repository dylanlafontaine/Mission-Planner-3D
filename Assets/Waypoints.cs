using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is going to be used to keep track of markers on the map
/// </summary>
public class Waypoints : MonoBehaviour
{
    private GameObject map;
    private OnlineMapsMarker3DManager mapMarkerManager;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map");
        if (map == null)
        {
            Debug.Log("Map game object not found.");
        }
        mapMarkerManager = map.GetComponent(typeof(OnlineMapsMarker3DManager)) as OnlineMapsMarker3DManager;
        if (mapMarkerManager == null)
        {
            Debug.Log("Marker Manage not found.");
        }
        Debug.Log("Marker system found.");
    }

    // Update is called once per frame
    void Update()
    {
        // add new waypoint
        if (Input.GetKey(KeyCode.P))
            if (!addWaypoint())
                Debug.Log("Failed to add waypoint");

        // TO DO: Delete waypoint, edit waypoint, move waypoint

        OnlineMaps.instance.Redraw();
    }

    private bool addWaypoint()
    {
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
        // Converts the screen coordinates to geographic.
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
        // Showing geographical coordinates in the console.
        //Debug.Log(mouseGeoLocation);

        // add waypoint
        //mapMarkerManager.CreateFromExistingGameObject(mouseGeoLocation[0], mouseGeoLocation[1], (Resources.Load("prefabs/ball", GameObject) as GameObject));
        return true;
    }
}
