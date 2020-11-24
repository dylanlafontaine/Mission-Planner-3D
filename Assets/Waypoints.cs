using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//NOTE Dylan L. -- This file has two classes in it which is bad OOP Practice
//NOTE Dylan L. -- I have no idea why every function returns a boolean value. I don't believe it is used anywhere

/// <summary>
///Commented by Dylan L.
///Waypoints -- This class is used to keep track of markers on the map. It defines all behavior for Waypoints GameObjects.
/// </summary>
public class Waypoints : MonoBehaviour
{
    //newSphere is not initialized here
    public GameObject newSphere;
    //prefabSphere is initialized to the MasterSphere GameObject which is a template for all spheres that are spawned.
    public GameObject prefabSphere;
    //points is initialized to a new List of Waypoint Objects
    public List<Waypoint> points = new List<Waypoint>();
    //remove is not initialized here
    private RemoveButtons remove;
    //pointCounter is initialized to 0
    private int pointCounter = 0;
    //timeElapsed is initialized to 0
    private float timeElapsed = 0;
    //altitudeValue is set to a constant 100
    public static int altitudeValue = 100;
    //boolean values for flags are set to false initially
    public static bool moveFlag = false;
    public static bool addFlag = true;
    public static bool insertFlag = false;
    //sphereRender is not initialized here
    Renderer sphereRender;

    // Start is called before the first frame update
    void Start()
    {
        //When the program first starts remove is set to the removeButtons Object
        remove = (RemoveButtons)FindObjectOfType(typeof(RemoveButtons));
    }

    //NOTE From Dylan L. -- I believe most conditionals that happen in this Update function should be put into their own functions
    //for readability
    // Update is called once per frame
    void Update()
    {
        //The following conditionals are checks to determine if a waypoint was failed to be added, moved, or inserted
        //They also call the functions that handle adding, moving, and inserting
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
                Debug.Log("Failed to insert waypoint");
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
                //NOTE From Dylan L. -- These functions referenced in the TODO have been completed but should they be placed here?
                // TO DO: Delete waypoint, edit waypoint, move waypoint

                // draw the line between the waypoint objects.
                drawLine();
    }

    ///<summary>
    ///drawLine -- When drawLine is called a line is drawn between all points on the map in order from 0 to n where n is the number of 
    ///points on the map. These lines allow the user to 
    ///</summary>
    private void drawLine()
    {
        //line is not initialized here
        LineRenderer line;
        //NOTE From Dylan L. -- This seems to be useless
        //for every point Object in the list of points a new line variable is initialized to a LineRenderer Object and their render value is set to false
        foreach (var point in points)
        {
            line = point.getGameObject().GetComponent(typeof(LineRenderer)) as LineRenderer;
            line.enabled = false;
        }
        //for every point in the list of points a line is created as a LineRenderer Object and it is connected from the Waypoint the program is on to 
        //the Waypoint following it until there are no waypoints left. The line's endWidth is smaller than its starting width so the user can tell the 
        //order of the waypoints.
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
    ///NOTE Dylan L -- Need to fix the display of the initial object so only the cloned object is displayed or create a shallow copy instead
    ///of a standard copy.
    /// addWaypoint -- When the user clicks the "P" key on their keyboard a new waypoint is added to the map.
    /// </summary>
    /// <param name="altitude">the altitude of the created waypoint</param>
    /// <returns>true upon success</returns>
    public bool addWaypoint(float altitude)
    {
        Debug.Log("adding waypoint");
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
		Debug.Log("mousePosition: " + mousePosition);
        //Screen coordinates of the cursor are converted to GeoCoordiates for easy OnlineMaps v3 integration
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
		Debug.Log("mouseGeoLocation: " + mouseGeoLocation);
        //mouseGeoLocation.z = 100;

        //Creates a new Waypoint from prefabSphere which is the MasterSphere GameObject
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        //Sets the sphere's name to the point counter's value
        newSphere.transform.name = (pointCounter++).ToString();
        //Adds a new LineRenderer Object to the new sphere and sets its width to 100 as default
        newSphere.AddComponent<LineRenderer>();
        newSphere.GetComponent<LineRenderer>().startWidth = 100;
        newSphere.GetComponent<LineRenderer>().endWidth = 100;
        //Initializes the Renderer of the newSphere to true
        Renderer newSphereRenderer = newSphere.GetComponent(typeof(Renderer)) as Renderer;
        newSphereRenderer.enabled = true;

        //Creates a new OnlineMapsMarker3D Object from the mouse's retrieved GeoLocation and attaches it to the sphere
        OnlineMapsMarker3D marker = OnlineMapsMarker3DManager.CreateItem(mouseGeoLocation, newSphere);
        //Sets the altitude type to be relative and sets the new Waypoint's altitude to the default of 100
        marker.altitudeType = OnlineMapsAltitudeType.relative;
        marker.altitude = altitude;

        //Creates a Waypoint object and adds it to the points list
        Waypoint point = new Waypoint(marker);
        point.Number = pointCounter;
        points.Add(point);

        //The OnlineMap is re-rendered after a point is added because the point added is now attached to the OnlineMaps Object
        OnlineMaps.instance.Redraw();
        //The UI is un-rendered if it is open
        remove.removeUI();
        //The status of the sphere is reset
        remove.resetSphereStatus();

        return true;
    }

    ///<summary>
    ///importWaypoint -- Called from importExportPoints script and allows the user to import a file that contains information about a point into the
    ///map when the user clicks the "Import" button on the UI
    ///</summary>
    public bool importWaypoint(int number, int frame, int command, decimal delay, decimal radius, decimal pass, decimal yaw, float longitude, float latitude, float altitude)
    {
        //Converts a command, which is a string description of what the point does, e.g. WAYPOINT, SPLINE_WAYPOINT, etc.
        string commandS = Waypoint.intToCommand(command);

        // Sets screens pointer's latitude and longitude
        Vector3 mouseGeoLocation = new Vector3(latitude, longitude);

        //Creates a new Waypoint from prefabSphere which is the MasterSphere GameObject
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        //Sets the sphere's name to the point counter's value
        newSphere.transform.name = (pointCounter++).ToString();
        //Adds a new LineRenderer Object to the new sphere and sets its width to 100 as default
        newSphere.AddComponent<LineRenderer>();
        newSphere.GetComponent<LineRenderer>().startWidth = 100;
        newSphere.GetComponent<LineRenderer>().endWidth = 100;
        //Initializes the Renderer of the newSphere to true
        Renderer newSphereRenderer = newSphere.GetComponent(typeof(Renderer)) as Renderer;
        newSphereRenderer.enabled = true;

        //Creates a new OnlineMapsMarker3D Object from the mouse's retrieved GeoLocation and attaches it to the sphere
        OnlineMapsMarker3D marker = OnlineMapsMarker3DManager.CreateItem(mouseGeoLocation, newSphere);
        //Sets the altitude type to be relative and sets the new Waypoint's altitude to the default of 100
        marker.altitudeType = OnlineMapsAltitudeType.relative;
        marker.altitude = altitude;

        //Creates a Waypoint object, sets its parameters to those passed into the function and adds it to the points list
        Waypoint point = new Waypoint(marker);
        point.Number = pointCounter;
        point.Frame = frame;
        point.Command = commandS;
        point.Delay = delay;
        point.Radius = radius;
        point.Pass = pass;
        point.Yaw = yaw;
        points.Add(point);

        //Re-renders the Map since a new point was added to it
        OnlineMaps.instance.Redraw();

        return true;
    }

    ///<summary>
    ///NOTE Dylan L. -- "DeleteWaypoint" script name should be changed to "deleteWaypoint" to remain consistent
    ///NOTE Dylan L. -- This function should also delete the Waypoint object that is then cloned. There are two copies of every Waypoint since
    ///the copy sent to OnlineMaps v3 is a deep copy instead of a shallow copy. Since deleteWaypoint doesn't delete every version of waypoint
    ///we need to add this functionality.
    ///deleteWaypoint -- Function is called from DeleteWaypoint Script Takes a Waypoint GameObject and deletes it from the map and list
    ///</summary>
    ///<param name="deletedPoint">the game object of one of our wapoints</param>
    ///<returns>true if success, false if failure.</returns>
    public bool deleteWaypoint(Waypoint deletedPoint)
    {
        //Removes Marker Object from the Map
        OnlineMapsMarker3DManager.RemoveItem(deletedPoint.Marker);
        //Removes the point deleted from the points List
        points.Remove(deletedPoint);
        //NOTE Dylan L. -- Keep in mind there is the old copy of the GameObject Waypoint still there
        //Destroys the GameObject Waypoint which is attached to the map
        Destroy(deletedPoint.getGameObject());
        remove.removeUI();
        remove.resetSphereStatus();

        //Redraws the Online Maps v3 Map since it has been updated
        OnlineMaps.instance.Redraw();
        return true;
    }

    ///<summary>
    ///setMoveFlag -- Function is called from MoveWaypoint script. Takes the user-selected waypoint, sets its color render to blue and removes the UI attached to the 
    ///selected waypoint
    ///</summary>
    public bool setMoveFlag(Waypoint selectedSphere)
    {
        //sets moveFlag to true and addFlag to false. These flags are used to determine which action the program should take when "P" is clicked
        moveFlag = true;
        addFlag = false;
        //Sets sphereRender to be the Renderer Object for the waypoint the user clicked on
        sphereRender = selectedSphere.getGameObject().GetComponent(typeof(Renderer)) as Renderer;
        //Sets the color of the waypoint to blue so the user can tell that this waypoint is the one to be moved
        sphereRender.material.color = Color.blue;
        //Removes the Waypoint pop-up UI from the selected waypoint
        remove.removeUI();
        return true;
    }


    ///<summary>
    ///NOTE Dylan L. -- Then only parameter of the old waypoint that the new one keeps is the altitude. This needs to be changed so everything is copied.
    ///Ideally it would be best to just move the existing object instead of deleting it and creating a new object.
    ///NOTE Dylan L. -- The new waypoint's name should remain the same as the old one's since they are supposed to be the same object
    ///moveWaypoint -- Called from Update function of Waypoints if the moveFlag is true. If the moveFlag is true then moveWaypoint will create a new waypoint at the 
    ///user's mouse location and then deletes the old waypoint. This new waypoint has the same altitude of the previous waypoint.
    ///<summary>
    public bool moveWaypoint(float altitude)
    {
        Debug.Log("moving waypoint");
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
		Debug.Log("mousePosition: " + mousePosition);
        //Screen coordinates of the cursor are converted to GeoCoordiates for easy OnlineMaps v3 integration
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
		Debug.Log("mouseGeoLocation: " + mouseGeoLocation);
        //mouseGeoLocation.z = 100;

        //Creates a new Waypoint from prefabSphere which is the MasterSphere GameObject
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        //Sets the sphere's name to the point counter's value
        newSphere.transform.name = (pointCounter++).ToString();
        //Adds a new LineRenderer Object to the new sphere and sets its width to 100 as default
        newSphere.AddComponent<LineRenderer>();
        newSphere.GetComponent<LineRenderer>().startWidth = 100;
        newSphere.GetComponent<LineRenderer>().endWidth = 100;
        Renderer newSphereRenderer = newSphere.GetComponent(typeof(Renderer)) as Renderer;
        newSphereRenderer.enabled = true;

        //Creates a new OnlineMapsMarker3D Object from the mouse's retrieved GeoLocation and attaches it to the sphere
        OnlineMapsMarker3D marker = OnlineMapsMarker3DManager.CreateItem(mouseGeoLocation, newSphere);
        //Sets the altitude type to be relative and sets the new Waypoint's altitude to the default of 100
        marker.altitudeType = OnlineMapsAltitudeType.relative;
        marker.altitude = altitude;

        //Creates a Waypoint object and adds it to the points list
        Waypoint point = new Waypoint(marker);
        //Creates a temp Waypoint object that is set to the waypoint the user selected for updating the new waypoint with the old one's values
        Waypoint temp = SpawnUI.selectedWaypoint;
        point.Number = pointCounter;
        point.Marker.altitude = temp.Marker.altitude;
        points[points.FindIndex(ind => ind.Equals(temp))] = point;
        deleteWaypoint(temp);

        //Online Maps v3 Map is re-rendered since a new point has been added to it
        OnlineMaps.instance.Redraw();
        //The UI is un-rendered if it is open
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
    }

    ///<summary>
    ///NOTE Dylan L. -- I don't think this function is actually used
    ///insertWaypoint -- Inserts a Waypoint Object into the middle of the points list instead of appending it to the list
    ///</summary>
    public bool insertWaypoint(float altitude)
    {
        Debug.Log("inserting waypoint");
        // Screen coordinate of the cursor.
        Vector3 mousePosition = Input.mousePosition;
        //Screen coordinates of the cursor are converted to GeoCoordiates for easy OnlineMaps v3 integration
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(mousePosition);
        //mouseGeoLocation.z = 100;

        //Creates a new Waypoint from prefabSphere which is the MasterSphere GameObject
        newSphere = Instantiate(prefabSphere, mouseGeoLocation, Quaternion.identity);
        //Sets the sphere's name to the point counter's value
        newSphere.transform.name = (pointCounter++).ToString();
        //Adds a new LineRenderer Object to the new sphere and sets its width to 100 as default
        newSphere.AddComponent<LineRenderer>();
        newSphere.GetComponent<LineRenderer>().startWidth = 100;
        newSphere.GetComponent<LineRenderer>().endWidth = 100;
        Renderer newSphereRenderer = newSphere.GetComponent(typeof(Renderer)) as Renderer;
        newSphereRenderer.enabled = true;
        
        //Creates a new OnlineMapsMarker3D Object from the mouse's retrieved GeoLocation and attaches it to the sphere
        OnlineMapsMarker3D marker = OnlineMapsMarker3DManager.CreateItem(mouseGeoLocation, newSphere);
        marker.altitudeType = OnlineMapsAltitudeType.relative;
        marker.altitude = altitude;

        //Creates a Waypoint object and adds it to the points list
        Waypoint point = new Waypoint(marker);
        point.Number = pointCounter;
        points.Insert(points.FindIndex(ind => ind.Equals(SpawnUI.selectedWaypoint)) + 1, point);

        //Online Maps v3 Map is re-rendered since a new point has been added to it
        OnlineMaps.instance.Redraw();
        //The UI is un-rendered if it is open
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
    }

    ///<summary>
    ///adjustAltitude -- Function called from InputAltitude Script. Sets the altitude of the selected point to the value specified by the user which is passed in as
    ///the "altitude" parameter
    ///</summary>
    public bool adjustAltitude(float altitude)
    {
		Debug.Log("adjusting altitude of waypoint");
        //The selected waypoint's Online Maps v3 Marker altitude is set to the user-specified input altitude
        SpawnUI.selectedWaypoint.Marker.altitude = altitude;
        //The updated waypoint is updated on the Online Maps v3 Map since its values were changed
        SpawnUI.selectedWaypoint.Marker.Update();
        //Removes the UI attached to the waypoint selected
        remove.removeUI();
        remove.resetSphereStatus();

        return true;
		
    }

    ///<summary>
    ///moveWaypointUp -- Function called from MoveUp script. The selected waypoint's index in the points list is moved up by one
    ///</summary>
    ///<param name="selectedWaypoint"></param>
    ///<returns></returns>
    public bool moveWaypointUp(Waypoint selectedWaypoint)
    {
        int index;
        Waypoint temp;
        //index is set to the selected waypoint's index value
        index = points.IndexOf(selectedWaypoint);
        //As long as index isn't the last element in the array the index of the selected waypoint is updated
        if (index < points.Count - 1)
        {
            //Swaps the selected waypoint and the next waypoint in the points list
            temp = points[index];
            points[index] = points[index + 1];
            points[index + 1] = temp;
            return true;
        }
        
        //Online Maps v3 Map is re-rendered since a new point has been added to it
        OnlineMaps.instance.Redraw();
        return false;
    }

    ///<summary>
    ///moveWaypointUp -- Function called from MoveUp script. The selected waypoint's index in the points list is moved down by one
    ///</summary>
    ///<param name="selectedWaypoint"></param>
    ///<returns></returns>
    public bool moveWaypointDown(Waypoint selectedWaypoint)
    {
        int index;
        Waypoint temp;
        //index is set to the selected waypoint's index value
        index = points.IndexOf(selectedWaypoint);
        //As long as index isn't the first element in the array the index of the selected waypoint is updated
        if (index > 0)
        {
            //Swaps the selected waypoint and the previous waypoint in the points list
            temp = points[index];
            points[index] = points[index - 1];
            points[index - 1] = temp;
            return true;
        }

        //Online Maps v3 Map is re-rendered since a new point has been added to it
        OnlineMaps.instance.Redraw();
        return false;
    }
}

///<summary>
///Waypoint -- This class defines how a single Waypoint should behave and what variables it should have
///</summary>
public class Waypoint
{
   
   //Waypoint (Constructor) -- Initializes a waypoint's variables with the values below
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

    //commandToInt -- Converts the command varaible to an integer value based off of the dictionary above 
    public int commandToInt()
    {
        return Commands[this._command];
    }

    //intToCommand -- Converts an integer value to a Comman string based off of the dictionary above
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
    //Returns the GameObject of the Online Maps v3 Marker
    public GameObject getGameObject ()
    {
        return(this._marker.instance);
    }

    //Returns the string that will be written to the outfile.
    public string export ()
    {
        //Creates a new StringBuilder object which is a mutable string of characters
        StringBuilder stringBuilder = new StringBuilder();
        //This string is populated with the Waypoint's variables
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