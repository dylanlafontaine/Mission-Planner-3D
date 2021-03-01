using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Command : MonoBehaviour
{
    public Button command, submit, cancel;
    public InputField inputField;
    public Dropdown dropdown;
    private Waypoints waypoints;
    public GameObject selectedSphere;
    public Waypoint selectedWaypoint;
    public RemoveButtons remove;
    // Start is called before the first frame update
    void Start()
    {
        command.onClick.AddListener(TaskOnClick);
        remove = (RemoveButtons)FindObjectOfType(typeof(RemoveButtons));
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    private void TaskOnClick()
    {
        remove.removeUI();
        //dropdown.transform.position = new Vector3(-254, 170, 0);
        //submit.transform.position = new Vector3(-294, 90, 0);
        //cancel.transform.position = new Vector3(-221, 90, 0);
        //inputField.transform.position = new Vector3(-253, 129, 0);
        dropdown.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 60, 0);
        int index = dropdown.options.FindIndex((i) => { return i.text.Equals(SpawnUI.selectedWaypoint.Command); });
        dropdown.value = index;
        submit.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 30, 0);
        cancel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 60, 0);
        inputField.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 30, 0);
        inputField.text = SpawnUI.selectedWaypoint.Delay.ToString();
    }

    public void cleanUpCommandUI()
    {
        Debug.Log("remove command called");
        dropdown.transform.position = new Vector3(-1000, 0, 0);
        submit.transform.position = new Vector3(-1000, 0, 0);
        cancel.transform.position = new Vector3(-1000, 0, 0);
        inputField.transform.position = new Vector3(-1000, 0, 0);
        inputField.textComponent.text = "";
    }
}
