using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class PlaceWaypointVR : MonoBehaviour
{
    public GameObject waypointPrefab;
    public Transform playerTransform;
    public Transform playerForwardTransform;
    public SpawnOnMap script;
    public AbstractMap map;
    private Vector2d latLong;
    private List<Vector2d> objectsToSpawn;
    public Waypoints waypoints;

    void Start() {
        waypoints = FindObjectOfType<Waypoints>();
        Invoke("SpawnWaypointFromList", 3);
    }

    public void SpawnWaypointFromList() {
        for (int i = 0; i < waypoints.points.Count; i++) {
            Vector2 position = waypoints.points[i].Marker.position;
            script.SpawnWaypointsOnMapFromString(position.y.ToString() + "," + position.x.ToString(), (float)waypoints.points[i].Marker.altitude);
        }
    }
    public void SpawnWaypoint() {
        latLong = playerTransform.GetGeoPosition(new Vector2d(map.CenterMercator.x, map.CenterMercator.y), 1);
        double x = latLong.x;
        double y = latLong.y;
        latLong.x = y;
        latLong.y = x;
        Debug.Log(latLong);
        script.SpawnWaypointsOnMap(latLong.ToString(), playerTransform, playerForwardTransform);
    }
}
