using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentListDisplay : MonoBehaviour
{
    public Transform targetTransform;
    public ContentListItemDisplay itemDisplayPrefab;
    private Waypoints waypoints;
    private List<Waypoint> currentList;

    // Start is called before the first frame update

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        currentList = waypoints.points;
    }

    void Update() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        if (!currentList.Equals(waypoints.points)) {
            foreach (Transform child in targetTransform) {
                Destroy(child.gameObject);
            }
            currentList = waypoints.points;
            Prime(waypoints.points);
        }
    }

    public void Prime(List<Waypoint> waypoints) {
        foreach (Waypoint waypoint in waypoints) {
            ContentListItemDisplay display = (ContentListItemDisplay)Instantiate(itemDisplayPrefab);
            display.transform.SetParent(targetTransform, false);
            display.InitWaypointDisplay(waypoint);
        }
    }
}
