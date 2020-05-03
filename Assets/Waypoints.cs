using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// This class is going to be used to keep track of markers on the map
/// </summary>
public class Waypoints : MonoBehaviour
{
    public GameObject newSphere;
    public GameObject prefabSphere;
    public List<Waypoint> points = new List<Waypoint>();
    private removeButtons remove;
    private int pointCounter = 0;
    private float timeElapsed = 0;
    public static int altitudeValue = 100;
    public static bool moveFlag = false;
    public static bool addFlag = true;
    public static bool insertFlag = false;
    Renderer sphereRender;

    // Start is called before the first frame update
    void Start()
    {
        remove = (removeButtons)FindObjectOfType(typeof(removeButtons));
    }

    // Update is called once per frame
    void Update()
    {
        // add new waypoint
        if (Time.time * 1000 - timeElapsed > 250 && Input.GetKey(KeyCode.P) && addFlag)
        {
            timeElapsed = Time.time * 1000;
            if (!addWaypoint((float)100.0))
                Debug.Log("Failed to add waypoint");
        }

        if (Time.time * 1000 - timeElapsed > 250 && Input.GetKey(KeyCode.P) && moveFlag)
        {
            timeElapsed = Time.time * 1000;
            if (!moveWaypoint((float)100.0))
                Debug.Log("Failed to move waypoint");
        }

        if (Time.time * 1000 - timeElapsed > 250 && Input.GetKey(KeyCode.P) && insertFlag)
        {
            timeElapsed = Time.time * 1000;
            if (!insertWaypoint((float)100.0))
                Debug.Log("Failed to move waypoint");
        }

        //if (Time.time * 1000 - timeElapsed > 250 && Input.GetKey(KeyCode.R))
        //{
        //    timeElapsed = Time.time * 1000;
        //    if (!adjustAltitude(altitudeValue))
        //        Debug.Log("Failed to move waypoint");
        //}

        //if (Time.time * 1000 - timeElapsed > 250 && Input.GetKey(KeyCode.F))
        //{
        //    timeElapsed = Time.time * 1000;
        //    if (!adjustAltitude(-altitudeValue))
        //        Debug.Log("Failed to move waypoint");
        //}

        // delete last waypoint that was added
        if (points.Count > 0)
            if (Input.GetKey(KeyCode.O))
                if (!deleteWaypoint(points[points.Count - 1]))
                    Debug.Log("Failed to delete waypoint");

        if (points.Count > 0)
            if (Input.GetKey(KeyCode.I))
                print(points[0].export());

                // TO DO: Delete waypoint, edit waypoint, move waypoint

                // draw the line between the waypoint objects.
                drawLine();
    }

    private void drawLine()
    {
        LineRenderer line;
        foreach (var point in points)
        {
            line = point.getGameObject().GetComponent(typeof(LineRenderer)) as LineRenderer;
            line.enabled = false;
        }
        for (int i = 0; i < points.Count - 1; i++)
        {
            line = points[i].getGameObject().GetComponent(typeof(LineRenderer)) as LineRenderer;
            line.enabled = true;
            line.SetPosition(0, points[i].getGameObject().transform.position);
            line.SetPosition(1, points[i + 1].getGameObject().transform.position);
            line.startWidth = 5f;
            line.endWidth = .1f;
        }
    }

    /// <summary>
    /// adds a new waypoint to the map.
    /// </summary>
    /// <param name="altitude">the altitude of the created waypoint</param>
    /// <returns>true upon success</returns>
    public bool addWaypoint(float altitude)
    {
        Debug.Log("adding waypoint");
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
		Debug.Log("mousePosition: " + mousePosition);
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
		Debug.Log("mouseGeoLocation: " + mouseGeoLocation);
        //mouseGeoLocation.z = 100;

        // should create a new marker
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        newSphere.transform.name = (pointCounter++).ToString();
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
        point.Number = pointCounter;
        points.Add(point);

        OnlineMaps.instance.Redraw();
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
    }

    public bool importWaypoint(int number, int frame, int command, decimal delay, decimal radius, decimal pass, decimal yaw, float longitude, float latitude, float altitude)
    {
        string commandS = Waypoint.intToCommand(command);

        // Screen set pointer lat lon
        Vector3 mouseGeoLocation = new Vector3(latitude, longitude);

        // should create a new marker
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        newSphere.transform.name = (pointCounter++).ToString();
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
        point.Number = pointCounter;
        point.Frame = frame;
        point.Command = commandS;
        point.Delay = delay;
        point.Radius = radius;
        point.Pass = pass;
        point.Yaw = yaw;
        points.Add(point);

        OnlineMaps.instance.Redraw();

        return true;
    }

    /// <summary>
    /// Takes a gameobject, should be one of our waypoints, and deletes it from the map, and list
    /// </summary>
    /// <param name="deletedPoint">the game object of one of our wapoints</param>
    /// <returns>true if success, false if failure.</returns>
    public bool deleteWaypoint(Waypoint deletedPoint)
    {
        OnlineMapsMarker3DManager.RemoveItem(deletedPoint.Marker); // remove the marker
        points.Remove(deletedPoint); // remove from the points list
        Destroy(deletedPoint.getGameObject()); // destroy the game object within points.
        remove.removeUI();
        remove.resetSphereStatus();

        OnlineMaps.instance.Redraw();
        return true;
    }

    public bool setMoveFlag(Waypoint selectedSphere)
    {
        moveFlag = true;
        addFlag = false;
        sphereRender = selectedSphere.getGameObject().GetComponent(typeof(Renderer)) as Renderer;
        sphereRender.material.color = Color.blue;
        remove.removeUI();
        return true;
    }

    public bool moveWaypoint(float altitude)
    {
        Debug.Log("moving waypoint");
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
		Debug.Log("mousePosition: " + mousePosition);
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
		Debug.Log("mouseGeoLocation: " + mouseGeoLocation);
        //mouseGeoLocation.z = 100;

        // should create a new marker
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        newSphere.transform.name = (pointCounter++).ToString();
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
        Waypoint temp = spawnUI.selectedWaypoint;
        point.Number = pointCounter;
        point.Marker.altitude = temp.Marker.altitude;
        points[points.FindIndex(ind => ind.Equals(temp))] = point;
        deleteWaypoint(temp);


        OnlineMaps.instance.Redraw();
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
    }

    public bool insertWaypoint(float altitude)
    {
        Debug.Log("inserting waypoint");
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
        //mouseGeoLocation.z = 100;

        // should create a new marker
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        newSphere.transform.name = (pointCounter++).ToString();
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
        point.Number = pointCounter;
        points.Insert(points.FindIndex(ind => ind.Equals(spawnUI.selectedWaypoint)) + 1, point);


        OnlineMaps.instance.Redraw();
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
    }

    public bool adjustAltitude(float altitude)
    {
		Debug.Log("adjusting altitude of waypoint");
        spawnUI.selectedWaypoint.Marker.altitude = altitude;
        spawnUI.selectedWaypoint.Marker.Update();
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
		
    }

    /// <summary>
    /// takes a waypoint object and moves it up in the list
    /// </summary>
    /// <param name="selectedWaypoint"></param>
    /// <returns></returns>
    public bool moveWaypointUp(Waypoint selectedWaypoint)
    {
        int index;
        Waypoint temp;
        index = points.IndexOf(selectedWaypoint);
        if (index < points.Count - 1)
        {
            temp = points[index];
            points[index] = points[index + 1];
            points[index + 1] = temp;
            return true;
        }

        OnlineMaps.instance.Redraw();
        return false;
    }

    /// <summary>
    /// takes a waypoint object and moves it down in the list. 
    /// </summary>
    /// <param name="selectedWaypoint"></param>
    /// <returns></returns>
    public bool moveWaypointDown(Waypoint selectedWaypoint)
    {
        int index;
        Waypoint temp;
        index = points.IndexOf(selectedWaypoint);
        if (index > 0)
        {
            temp = points[index];
            points[index] = points[index - 1];
            points[index - 1] = temp;
            return true;
        }

        OnlineMaps.instance.Redraw();
        return false;
    }
}

public class Waypoint
{
    /**********************************************************************************
     * CONSTRUCTOR
    ***********************************************************************************/
    public Waypoint(OnlineMapsMarker3D marker)
    {
        this._marker = marker;
        this._number = 0;
        this._frame = 0;
        this._command = "WAYPOINT";
        this._delay = (decimal) 0.0;
        this._radius = (decimal)0.0;
        this._pass = (decimal)0.0;
        this._yaw = (decimal)0.0;
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
    public static Dictionary<string, int> Commands = new Dictionary<string, int> {
        {"WAYPOINT", 16},
        {"SPLINE_WAYPOINT", 82},
        {"LOITER_TURNS", 18},
        {"LOITER_TIME", 19},
        {"LOITER_UNLIM", 17},
        {"RETURN_TO_LAUNCH", 20},
        {"LAND", 21},
        {"TAKEOFF", 22},
        {"DELAY", 93},
        {"GUIDED_ENABLE", 92},
        {"PAYLOAD_PLACE", 94},
        {"DO_GUIDED_LIMITS", 222},
        {"DO_SET_ROI", 201},
        {"CONDITION_DELAY", 112},
        {"CONDITION_CHANGE_ALT", 113},
        {"CONDITION_DISTANCE", 114},
        {"CONDITION_YAW", 115},
        {"DO_JUMP", 177},
        {"DO_CHANGE_SPEEDS", 178},
        {"DO_GRIPPER", 211},
        {"DO_PARACHUTE", 208},
        {"DO_SET_CAM_TRIGG_DIST", 206},
        {"DO_SET_RELAY", 181},
        {"DO_SET_SERVO", 183},
        {"DO_REPEAT_SERVO", 184},
        {"DO_DIGICAM_CONFIGURE", 202},
        {"DO_DIGICAM_CONTROL", 203},
        {"DO_MOUNT_CONTROL", 205},
        {"UNKOWN", 999}
    };

    private string _command;

    public string Command
    {
        get
        {
            return this._command;
        }
        set
        {
            this._command = value;
        }
    }

    public int commandToInt()
    {
        return Commands[this._command];
    }

    public static string intToCommand(int comm)
    {
        foreach (KeyValuePair<string, int> iter in Commands) 
            if (iter.Value == comm)
                return (iter.Key);
        return "WAYPOINT";
    }

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


    /**********************************************************************************
     * FUNCTIONS
    ***********************************************************************************/
    // returns the game object of the marker
    public GameObject getGameObject ()
    {
        return(this._marker.instance);
    }

    // will return the string that will be written to the outfile.
    public string export ()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(this.Number.ToString() + "\t" + 
            this.Placeholder0.ToString() + "\t" + 
            this.Frame.ToString() + "\t" +
            Waypoint.Commands[this.Command].ToString() + "\t" +
            this.Delay.ToString() + "\t" +
            this.Radius.ToString() + "\t" +
            this.Pass.ToString() + "\t" + 
            this.Yaw.ToString() + "\t" + 
            this.Marker.position[1].ToString() + "\t" + 
            this.Marker.position[0].ToString() + "\t" +
            this.Marker.altitude.ToString() + "\t" +
            this._placeholder1.ToString());

        return (stringBuilder.ToString());
    }
}