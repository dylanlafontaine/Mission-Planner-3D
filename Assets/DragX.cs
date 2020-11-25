using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragX : MonoBehaviour
{
    private Waypoint marker;
    private Waypoints waypoints;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
    }

   private void OnMouseDrag() {
       double longitude, lat;
       string parentName;
       OnlineMapsTileSetControl.instance.allowUserControl = false;
       parentName = transform.parent.name;
       foreach (var point in waypoints.points) {
           if (point.getGameObject().name == parentName) {
               marker = point;
               break;
           }
       }
       //OnlineMapsMarker3DManager
       marker.Marker.GetPosition(out longitude, out lat);
       Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + transform.position.z);
       Vector3 geoMousePosition = OnlineMapsControlBase.instance.GetCoords(mousePosition);
       marker.Marker.SetPosition(geoMousePosition.x, lat);
       //Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
       //transform.position = objPosition;
       //sphere.isKinematic = true;
   }

   private void OnMouseUp() {
       OnlineMapsTileSetControl.instance.allowUserControl = true;
   }
}
