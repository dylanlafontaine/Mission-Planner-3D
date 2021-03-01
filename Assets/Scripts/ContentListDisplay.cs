using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentListDisplay : MonoBehaviour
{
    public Transform targetTransform;
    public ContentListItemDisplay itemDisplayPrefab;
    private Waypoints waypoints;
    private int currentPointsCount;

    // Start is called before the first frame update

    void Start()
    {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        currentPointsCount = waypoints.points.Count;
    }

    void Update()
    {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        if (currentPointsCount != waypoints.points.Count)
        {
            currentPointsCount = waypoints.points.Count;
            Prime(waypoints.points);
        }
    }

    public void Prime(List<Waypoint> waypoints)
    {
        foreach (Transform child in targetTransform)
        {
            if (child.name != "ContentListHeader")
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Waypoint waypoint in waypoints)
        {
            ContentListItemDisplay display = (ContentListItemDisplay)Instantiate(itemDisplayPrefab);
            display.transform.SetParent(targetTransform, false);
            display.InitWaypointDisplay(waypoint);
        }
    }
}
