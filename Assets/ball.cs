using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // This object was clicked - do something
    void OnMouseDown()
    {
        var rand = new System.Random();
        transform.position = transform.position + new Vector3(rand.Next(3), rand.Next(3), rand.Next(3));
    }
}
