using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is going to be used to keep track of markers on the map
/// </summary>
public class Waypoints : MonoBehaviour
{
    public GameObject newSphere;
    public GameObject prefabSphere;
    public List<Waypoint> points = new List<Waypoint>();
    private MasterController master;

    // Start is called before the first frame update
    void Start()
    {

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

    public bool addWaypoint(float altitude)
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

        OnlineMaps.instance.Redraw();

        return true;
    }

    // deletes the selected game object from the waypoints class when called
    public bool deleteWaypoint()
    {

        return false;
    }
}

public class Waypoint
{
    /**********************************************************************************
     * CONSTRUCTOR
    ***********************************************************************************/
    public OnlineMapsMarker3D Marker
    {
        get
        {
            return this._marker;
        }
        set
        {
            this._marker = value;
        }
    }


    /***********************************************************************************
     * PARAMETERS: These are the variables and properties for all of the editor properties
     * for the waypoints. Use these to edit the wayboint object in functions in the
     * Waypoints class.
     * PARAMS ordered left to right as seen in mission planner output file for waypoints
     * 
    ************************************************************************************/
    //PARAM 1
    //Waypoint number in order of creation (0 is home point, counting starts at 1)
    private int _number;

    public int Number
    {
        get
        {
            return this._number;
        }
        set
        {
            this._number = value;
        }
    }

    //PARAM 2
    //IDK BUT THIS VALUE IS ALWAYS 0
    // -> homebutton value here is 1
    private int _placeholder0 = 0;

    public int Placeholder0
    {
        get
        {
            return 0;
        }
    }

    //PARAM 3
    //Frame - 3 possible outpoints referring to point's position
    // (absolute = 0, relative = 3, terrain = 10)
    // -> homebutton default value is 0
    private int _frame;

    public int Frame 
    {
        get 
        {
            return this._frame;
        }
        set
        {
            this._frame = value;
        }
    }

    //PARAM 4
    //Command - many options, important seem to be takeoff, land, waypoint
    // this also affects the next few arguments, especially delay / radius / pass / yaw?
    // I also believe these change based on what setting you are in, i.e copter / plane / etc...
    // table --------------------------------
    /*
            name                value
            WAYPOINT                16
            SPLINE_WAYPOINT         82
            LOITER_TURNS            18
            LOITER_TIME             19
            LOITER_UNLIM            17
            RETURN_TO_LAUNCH        20
            LAND                    21
            TAKEOFF                 22
            DELAY                   93
            GUIDED_ENABLE           92
            PAYLOAD PLACE           94
            DO_GUIDED_LIMITS        222
            DO_SET_ROI              201
            CONDITION_DELAY         112
            CONDITION_CHANGE_ALT    113
            CONDITION_DISTANCE      114
            CONDITION_YAW           115
            DO_JUMP                 177
            DO_CHANGE_SPEED         178
            DO_GRIPPER              211
            DO_PARACHUTE            208
            DO_SET_CAM_TRIGG_DIST   206
            DO_SET_RELAY            181
            DO_SET_SERVO            183
            DO_REPEAT_SERVO         184
            DO_DIGICAM_CONFIGURE    202
            DO_DIGICAM_CONTROL      203
            DO_MOUNT_CONTROL        205
            UNKOWN                  CAN SET TO WHATEVER VALUE YOU WANT
    */
    private int _command;

    // PARAM 5
    // Delay - seconds, may change based on what command, default is delay for waypoint
    private decimal _delay;

    public decimal Delay
    {
        get
        {
            return this._delay;
        }
        set
        {
            this._delay = value;
        }
    }

    // PARAM 6
    // acceptance radius in meters 
    // (when plain inside the sphere of this radius, the waypoint is considered reached) (plane only)
    private decimal _radius;

    public decimal Radius
    {
        get
        {
            return this._radius;
        }
        set
        {
            this._radius = value;
        }
    }

    // PARAM 7
    // 0 to pass through the WP, if > 0 radius in meters to pass by WP. Positive value for
    // clockwise orbit, negative value for counter-clockwise orbit. Allows trajectory control.
    private decimal _pass;

    public decimal Pass
    {
        get
        {
            return this._pass;
        }
        set
        {
            this._pass = value;
        }
    }

    // PARAM 8
    // desired yaw angle at waypoint target (rotary wing)
    private decimal _yaw;

    public decimal Yaw
    {
        get
        {
            return(this._yaw);
        }
        set
        {
            this._yaw = value;
        }
    }

    // PARAM 9
    // Lat - Target latitude. If 0, the Copter will hold at current latitude
    // STORED IN MARKER!!!!!!!!!!!!!!

    // PARAM 10
    // Long - Target longitude. If 0, the Copter will hold at current longitude
    // STORED IN MARKER!!!!!!!!!!!!!!

    // PARAM 11
    // Alt - Target altitude. If 0, the Copter will hold at current altitude
    // a "Landing" command value sets this to 0 automatically
    // STORED IN MARKER!!!!!!!!!!!!!!

    // PARAM 12
    // IDK BUT THIS VALUE IS ALWAYS 1
    private int _placeholder1 = 1;

    // MARKER!
    // contains the rendered marker game object, as well as the geo location,
    // altitude, and more,
    private OnlineMapsMarker3D _marker;

    public Waypoint(OnlineMapsMarker3D marker)
    {
        this._marker = marker;
    }


    /**********************************************************************************
     * FUNCTIONS
    ***********************************************************************************/
    // returns the game object of the marker
    public GameObject getGameObject ()
    {
        return(this._marker.instance);
    }

    // will return the string that will be written to the outfile.
    public string buildLine ()
    {
        return("a line of waypoint data");
    }
}