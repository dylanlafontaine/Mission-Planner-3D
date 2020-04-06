﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //GameObject map = GetComponent<GameObject>("Map");
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer line;
        foreach (var point in points)
        {
            line = point.GetComponent(typeof(LineRenderer)) as LineRenderer;
            line.enabled = false;
        }
        for (int i = 0; i < points.Count - 1; i++)
        {
            line = points[i].GetComponent(typeof(LineRenderer)) as LineRenderer;
            line.enabled = true;
            line.SetPosition(0, points[i].transform.position);
            line.SetPosition(1, points[i + 1].transform.position);
            line.startWidth = 5f;
            line.endWidth = .1f;
        }
    }
}
