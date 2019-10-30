using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readSpawnTest : MonoBehaviour
{
    GeoCoordinate test = new GeoCoordinate();
    public GameObject myPrefab;
    public List<GameObject> points = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        string line;
        int[] nums = new int[4];
        // Read the file and display it line by line.  
        System.IO.StreamReader file =
            new System.IO.StreamReader(@"points.txt");
        while ((line = file.ReadLine()) != null)
        {
            string[] words = line.Split(' ');
            for(int i = 0; i < 3; i++)
            {
                //Debug.Log(words[i]);
                nums[i] = Int32.Parse(words[i]);
                //Debug.Log(nums[i]);
            }
            GameObject point = Instantiate(myPrefab, new Vector3(nums[1], nums[2], nums[3]), Quaternion.identity);
            point.AddComponent<LineRenderer>();
            points.Add(point);
            //Debug.Log(line);
        }
        Debug.Log("Up and running");
        Renderer masterPointRenderer = myPrefab.GetComponent(typeof(Renderer)) as Renderer;
        masterPointRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer line;
        for (int i = 0; i < points.Count - 1; i++)
        {
            line = points[i].GetComponent(typeof(LineRenderer)) as LineRenderer;
            line.SetPosition(0, points[i].transform.position);
            line.SetPosition(1, points[i + 1].transform.position);
            line.startWidth = 5f;
            line.endWidth = .1f;
        }
    }
}
