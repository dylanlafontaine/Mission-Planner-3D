using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragZ : MonoBehaviour
{
    private Waypoint marker;
    private Waypoints waypoints;
    private ContentListDisplay display;

    void Start() {
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        display = (ContentListDisplay)FindObjectOfType<ContentListDisplay>();
    }

    void OnBecameInvisible() {
        if (OnlineMapsTileSetControl.instance.allowUserControl == false) {
            OnlineMapsTileSetControl.instance.allowUserControl = true;
        }
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
       Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + transform.position.z);
       Vector3 geoMousePosition = OnlineMapsControlBase.instance.GetCoords(mousePosition);
       marker.Marker.SetPosition(longitude, geoMousePosition.y);
       display.Prime(waypoints.points);
   }

   private void OnMouseUp() {
       OnlineMapsTileSetControl.instance.allowUserControl = true;
   }
}
