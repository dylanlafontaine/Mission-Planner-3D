using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This object was clicked - do something
    void OnMouseDown()
    {
        var rand = new System.Random();
        transform.position = transform.position + new Vector3(rand.Next(3), rand.Next(3), rand.Next(3));
    }
}
