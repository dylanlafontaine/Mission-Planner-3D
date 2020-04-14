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
    public GameObject newSphere;
    public GameObject prefabSphere;
    public List<Waypoint> points = new List<Waypoint>();
    private MasterController master;

    // Start is called before the first frame update
    void Start()
    {
        master = (MasterController)FindObjectOfType(typeof(MasterController));

    }

    // Update is called once per frame
    void Update()
    {
        // add new waypoint
        if (Input.GetKey(KeyCode.P))
            if (!addWaypoint((float)100.0))
                Debug.Log("Failed to add waypoint");

        // TO DO: Delete waypoint, edit waypoint, move waypoint
    }

    private bool addWaypoint(float altitude)
    {
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
        mouseGeoLocation.z = 100;
        // should create a new marker
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        newSphere.transform.name = "Sphere";
        newSphere.AddComponent<LineRenderer>();
        newSphere.GetComponent<LineRenderer>().startWidth = 100;
        newSphere.GetComponent<LineRenderer>().endWidth = 100;
        Renderer newSphereRenderer = newSphere.GetComponent(typeof(Renderer)) as Renderer;
        newSphereRenderer.enabled = true;
        OnlineMapsMarker3D marker = OnlineMapsMarker3DManager.CreateItem(mouseGeoLocation, newSphere);
        marker.altitudeType = OnlineMapsAltitudeType.relative;
        marker.altitude = altitude;
        // create waypoint object and add it to list
        Waypoint point = new Waypoint(marker);
        points.Add(point);
        master.points.Add(newSphere);
        Debug.Log(points);

        OnlineMaps.instance.Redraw();

        return true;
    }
}

public class Waypoint
{
    private OnlineMapsMarker3D _marker;

    public Waypoint(OnlineMapsMarker3D marker)
    {
        this._marker = marker;
    }

    public OnlineMapsMarker3D Marker {
        get {
            return this._marker;
        }
        set {
            this._marker = value;
        }
    }

    public GameObject getGameObject ()
    {
        return this._marker.instance;
    }
}