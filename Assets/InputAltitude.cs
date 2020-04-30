using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputAltitude : MonoBehaviour
{
    public InputField myButton;
    public Waypoints waypoints;
    public removeButtons remove;
    // Start is called before the first frame update
    void Start()
    {
        //myButton.onValueChange.AddListener(delegate { UpdateAltitude(myButton.text); });
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
        remove = (removeButtons)FindObjectOfType(typeof(removeButtons));
    }

    // Update is called once per frame
    void Update()
    {
        if (myButton.isFocused && myButton.text != "" && Input.GetKey(KeyCode.Return))
        {
            UpdateAltitude(myButton.text);
            myButton.text = "";
        }
    }

    void UpdateAltitude(string input)
    {
        float parse;
        if (float.TryParse(input, out parse))
        {
            waypoints.adjustAltitude(parse);
        }
        else
            remove.removeUI();
    }
}
