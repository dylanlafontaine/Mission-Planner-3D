using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }
}
