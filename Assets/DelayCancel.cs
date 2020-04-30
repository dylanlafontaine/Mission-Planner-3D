using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DelayCancel : MonoBehaviour
{
    public Button myButton;
    public InputField inputField;
    public Command command;
    public Waypoints waypoints;
    public Dropdown dropdown;
    public removeButtons remove;
    // Start is called before the first frame update
    void Start()
    {
        command = (Command)FindObjectOfType(typeof(Command));
        remove = (removeButtons)FindObjectOfType(typeof(removeButtons));
    }

    // Update is called once per frame
    void Update()
    {
        myButton.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        command.cleanUpCommandUI();
        remove.resetSphereStatus();
    }
}
