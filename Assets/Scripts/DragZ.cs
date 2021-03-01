using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DragZ : MonoBehaviour
{
    private Waypoint marker;
    private Waypoints waypoints;
    private ContentListDisplay display;
    private MarkerCollided detectCollisionScript;
    public GameObject markerCollider;
    private double longitude, latitude;
    private Vector3 geoMousePosition;
    private bool lockMovement;

    void Start()
    {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        display = (ContentListDisplay)FindObjectOfType<ContentListDisplay>();
    }


    void OnBecameInvisible()
    {
        if (OnlineMapsTileSetControl.instance.allowUserControl == false)
        {
            OnlineMapsTileSetControl.instance.allowUserControl = true;
            OnlineMapsControlBase.instance.lockYAxis = false;
        }
    }

    private void OnMouseDrag()
    {
        string parentName;
        parentName = transform.parent.name;
        foreach (var point in waypoints.points)
        {
            if (point.getGameObject().name == parentName)
            {
                marker = point;
                break;
            }
        }
        OnlineMapsTileSetControl.instance.allowUserControl = false;
        OnlineMapsControlBase.instance.dragMarker = marker.Marker;
        OnlineMapsControlBase.instance.markerParent = transform.parent.gameObject;
        OnlineMapsControlBase.instance.lockYAxis = true;
        display.Prime(waypoints.points);
    }

    private void OnMouseUp()
    {
        OnlineMapsControlBase.instance.lockYAxis = false;
        OnlineMapsTileSetControl.instance.allowUserControl = true;
        lockMovement = false;
    }
}