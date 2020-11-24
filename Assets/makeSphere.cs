﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MakeSphere : MonoBehaviour
{
    public GameObject newSphere;
    public GameObject prefabSphere;
    public Button myButton;
    private Camera cam;
    private MasterController master;
    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
        cam = Camera.main;
        master = (MasterController)FindObjectOfType(typeof(MasterController));
    }

    void TaskOnClick()
    {
        Vector3 point;
        //Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        Vector3 mousePositionInWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePositionInWorld.y = 0;
        Debug.Log("clicked");
        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        //mousePos.x = Input.mousePosition.x;
        //mousePos.y = cam.pixelHeight - Input.mousePosition.y;
        point = new Vector3(SpawnUI.selectedSphere.position.x, SpawnUI.selectedSphere.position.y + 10, SpawnUI.selectedSphere.position.z);
        //point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane + 50));
        //Debug.Log("position: " + cam.nearClipPlane);
        //newSphere = Instantiate(prefabSphere, mousePositionInWorld, Quaternion.identity);
        newSphere = Instantiate(prefabSphere, point, Quaternion.identity);
        newSphere.transform.name = "Sphere";
        Renderer newSphereRenderer = newSphere.GetComponent(typeof(Renderer)) as Renderer;
        newSphereRenderer.enabled = true;

        newSphere.AddComponent<LineRenderer>();
        master.points.Add(newSphere);
        //newSphere.AddComponent<SpawnUI>();
        //GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        //GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        //GUILayout.Label("Mouse position: " + mousePos);
        //GUILayout.Label("World position: " + point.ToString("F3"));
        //GUILayout.EndArea();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit\n");
        CheckUIHover.BlockedByUI = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter\n");
        CheckUIHover.BlockedByUI = true;
    }
}
