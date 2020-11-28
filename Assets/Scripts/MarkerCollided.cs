using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerCollided : MonoBehaviour
{
    public bool collision;
    private Waypoints waypoints;

    void OnTriggerStay(Collider collidedObject)
    {
        if (collidedObject.tag != "Arrow" && collidedObject.tag != "Map")
        {
            Debug.Log("Collision is True");
            collision = true;
        }
    }

    void OnTriggerExit(Collider collidedObject)
    {
        if (collidedObject.tag != "Arrow" && collidedObject.tag != "Map")
        {
            collision = false;
        }
    }
}
