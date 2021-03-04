using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentListItemDisplay : MonoBehaviour
{
    public Text waypointListNum;
    public Text waypointNum;
    public Dropdown commandDropdown;
    public InputField input1;
    public InputField input2;
    public InputField input3;
    public InputField input4;
    public InputField latInput;
    public InputField longInput;
    public InputField altInput;
    public Button deleteButton;
    public Button upButton;
    public Button downButton;
    public Text gradText;
    public Text distText;
    public Text azText;
    protected Waypoint waypoint;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoint != null)
        {
            InitWaypointDisplay(waypoint, 0);
        }
    }

    public void InitWaypointDisplay(Waypoint waypoint, int i)
    {
        double latitude, longitude;
        this.waypoint = waypoint;
        waypoint.Marker.GetPosition(out longitude, out latitude);
        waypointNum.text = waypoint.Number.ToString();
        waypointNum.gameObject.SetActive(false);
        waypointListNum.text = i.ToString();
        input1.text = "0";
        input1.interactable = false;
        input2.text = "0";
        input2.interactable = false;
        input3.text = "0";
        input3.interactable = false;
        input4.text = "0";
        input4.interactable = false;
        latInput.text = latitude.ToString();
        longInput.text = longitude.ToString();
        altInput.text = waypoint.Marker.altitude.ToString();
        gradText.text = "0%";
        distText.text = "0";
        azText.text = "0";
    }
}
