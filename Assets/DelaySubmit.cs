using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DelaySubmit : MonoBehaviour
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
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
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
        float parse;
        Debug.Log(spawnUI.selectedWaypoint.getGameObject().name);
        Debug.Log("input: " + inputField.textComponent.text);
        if (float.TryParse(inputField.text, out parse))
        {
            if (spawnUI.selectedWaypoint == null)
                Debug.Log("null");
            Debug.Log("succeeded parse");
            spawnUI.selectedWaypoint.Delay = (decimal)parse;
        }
        else
        {
            if (spawnUI.selectedWaypoint == null)
                Debug.Log("null");
            Debug.Log("failed parse");
            spawnUI.selectedWaypoint.Delay = (decimal)0.0;
        }
        Debug.Log(dropdown.captionText.text);
        spawnUI.selectedWaypoint.Command = dropdown.captionText.text;
        remove.resetSphereStatus();
    }
}
