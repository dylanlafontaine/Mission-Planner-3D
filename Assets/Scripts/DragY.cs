using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragY : MonoBehaviour
{
    private Waypoint marker;
    private Waypoints waypoints;
    private float zCoord;
    private ContentListDisplay display;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        display = (ContentListDisplay)FindObjectOfType<ContentListDisplay>();
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
       marker.Marker.GetPosition(out longitude, out lat);
       Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y - Camera.main.WorldToScreenPoint(gameObject.transform.position).z);
       if (OnlineMapsCameraOrbit.instance.rotation.x >= 60) {
        marker.Marker.altitude = mousePosition.z;
       }
       Debug.Log(marker.Marker.altitude);
       OnlineMaps.instance.Redraw();
       display.Prime(waypoints.points);
   }

   private void RestrictTilt() {
       if (OnlineMapsCameraOrbit.instance.rotation.x < 60) {
           OnlineMapsCameraOrbit.instance.rotation.x = 60;
       }
   }

   private void OnMouseUp() {
       OnlineMapsTileSetControl.instance.allowUserControl = true;
   }
}
