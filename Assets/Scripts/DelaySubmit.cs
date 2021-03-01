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
    public RemoveButtons remove;
    // Start is called before the first frame update
    void Start()
    {
        command = (Command)FindObjectOfType(typeof(Command));
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
        remove = (RemoveButtons)FindObjectOfType(typeof(RemoveButtons));
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
        Debug.Log(SpawnUI.selectedWaypoint.getGameObject().name);
        Debug.Log("input: " + inputField.textComponent.text);
        if (float.TryParse(inputField.text, out parse))
        {
            if (SpawnUI.selectedWaypoint == null)
                Debug.Log("null");
            Debug.Log("succeeded parse");
            SpawnUI.selectedWaypoint.Delay = (decimal)parse;
        }
        else
        {
            if (SpawnUI.selectedWaypoint == null)
                Debug.Log("null");
            Debug.Log("failed parse");
            SpawnUI.selectedWaypoint.Delay = (decimal)0.0;
        }
        Debug.Log(dropdown.captionText.text);
        SpawnUI.selectedWaypoint.Command = dropdown.captionText.text;
        remove.resetSphereStatus();
    }
}
