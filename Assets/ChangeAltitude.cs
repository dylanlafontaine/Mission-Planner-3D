using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChangeAltitude : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField altInput;
    public Button myButton;
    public Waypoints waypoints;
    public static double input = 0;
    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        altInput.transform.position = myButton.transform.position;
    }
}
