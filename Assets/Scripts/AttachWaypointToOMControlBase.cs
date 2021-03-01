using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachWaypointToOMControlBase : MonoBehaviour
{
    private void OnMouseOver() {
        if (!Input.GetMouseButtonDown(0)) {
            return;
        }
        OnlineMapsControlBase.instance.markerParent = transform.gameObject;
    }
}
