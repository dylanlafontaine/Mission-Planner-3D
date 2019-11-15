using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class readSpawnTest : MonoBehaviour
{
    GeoCoordinate test = new GeoCoordinate();
    public GameObject masterPoint;
    public GameObject originPoint;
    GeoCoordinate origin = new GeoCoordinate();
    public List<GameObject> points = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        origin.Latitude = 46.7302976970894;
        origin.Longitude = -117.168948054314;
        CoordinateConverter myConverter = new CoordinateConverter();
        GeoCoordinate newPoint = new GeoCoordinate();
        MeterCoordinate result = new MeterCoordinate();
        string line;
        int lineNum = 1;
        double[] nums = new double[20];
        string[] words = { };
        // Read the file and display it line by line.  
        System.IO.StreamReader file =
            new System.IO.StreamReader(@"SloanTest.waypoints");
        //Ignoring header
        if ((line = file.ReadLine()) == null)
            Debug.Log("Blank File!!!");
        //Ignoring sealevel?
        if ((line = file.ReadLine()) == null)
            Debug.Log("Blank File!!!");
        while ((line = file.ReadLine()) != null)
        {
            Debug.Log("New point #" + lineNum);
            //Debug.Log(line);
            words = Regex.Split(line, "\t");
            //Debug.Log("words.Length = " + words.Length);
            int i = 0;
            foreach (string word in words)
            {
                //Debug.Log((string)word);
                nums[i] = double.Parse(word);
                /*if (i == 10)
                {
                    Debug.Log(word);
                    Debug.Log(nums[i]);
                }*/
                i++;

            }
            //lat = nums[8]
            newPoint.Latitude = nums[8];
            //long = nums[9]
            newPoint.Longitude = nums[9];
            //height = nums[10]
            Debug.Log("Origin Lat = " + origin.Latitude + "\nnewPoint Lat = " + newPoint.Latitude);
            Debug.Log("Origin Long = " + origin.Longitude + "\nnewPoint Long = " + newPoint.Longitude);
            result = myConverter.FindMeterCoordinateFromOrigin(origin, newPoint);
            Debug.Log("New point X = " + (float)result.X);
            Debug.Log("New point Y = " + (float)nums[10]);
            Debug.Log("New point Z = " + (float)result.Y);
            Debug.Log("New point final X = " + (originPoint.transform.position.x + (float)result.X));
            Debug.Log("New point final Y = " + (originPoint.transform.position.y + (float)nums[10]));
            Debug.Log("New point final Z = " + (originPoint.transform.position.z + (float)result.Y));
            GameObject point = Instantiate(masterPoint, new Vector3(originPoint.transform.position.x + (float)result.X, originPoint.transform.position.y + (float)nums[10], 
                originPoint.transform.position.z + (float)result.Y), Quaternion.identity);
            point.transform.name = "Sphere";
            point.AddComponent<LineRenderer>();
            points.Add(point);
            Debug.Log("");
            Array.Clear(words, 0, 11);
            lineNum++;

        }
        Debug.Log("Up and running");
        Renderer masterPointRenderer = masterPoint.GetComponent(typeof(Renderer)) as Renderer;
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
