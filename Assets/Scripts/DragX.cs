﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DragX : MonoBehaviour
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
        OnlineMapsControlBase.instance.lockXAxis = true;
    }

    private void OnMouseUp()
    {
        OnlineMapsControlBase.instance.lockXAxis = false;
        OnlineMapsTileSetControl.instance.allowUserControl = true;
        lockMovement = false;
    }
}