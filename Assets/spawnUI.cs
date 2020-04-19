using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnUI : MonoBehaviour
{
    public Button deleteButton, addButton, moveUp, moveDown;
    public bool deleteInScreen;
    public static Transform selectedSphere;
    public static Waypoint selectedWaypoint;
    public static bool onSphere;
    public Renderer sphereRender;
    private bool isWaypoint;
    private Waypoints waypoints;

    // Start is called before the first frame update
    void Start()
    {
        deleteInScreen = false;
        onSphere = false;
        waypoints = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CheckUIHover.BlockedByUI)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                isWaypoint = false;
                foreach(var point in waypoints.points)
                {
                    if (point.getGameObject().name == hit.transform.name)
                    {
                        isWaypoint = true;
                        selectedWaypoint = point;
                        break;
                    }
                }
                if (isWaypoint)
                {
                    selectedSphere = hit.transform;
                    deleteInScreen = true;
                    //Debug.Log("My object is clicked by mouse");
                    deleteButton.transform.position = new Vector3(Input.mousePosition.x - 50, Input.mousePosition.y, 0);
                    addButton.transform.position = new Vector3(Input.mousePosition.x + 50, Input.mousePosition.y, 0);
                    moveUp.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 50, 0);
                    moveDown.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 50, 0);
                    onSphere = true;
                    sphereRender = selectedSphere.GetComponent(typeof(Renderer)) as Renderer;
                    sphereRender.material.color = Color.green;
                    ControlSphere.mySphere = hit.transform;
                    Debug.Log("on sphere");
                }
                else
                {
                    onSphere = false;
                }
            }
            else
            {
                onSphere = false;
            }
        }
        if (selectedSphere == null)
            onSphere = false;
    }
}
