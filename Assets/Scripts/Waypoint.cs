using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
        this._radius = (decimal)1.0;
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
    private int _i;
    public int I {
        get {
            return this._i;
        }
        set {
            this._i = value;
        }
    }
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