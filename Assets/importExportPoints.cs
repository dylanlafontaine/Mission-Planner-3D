using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class importExportPoints : MonoBehaviour
{
    public List<GameObject> points;

    public Button myButton;
    public GameObject masterPoint;
    public GameObject originPoint;
    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(Import);
        points = ((MasterController)FindObjectOfType(typeof(MasterController))).points;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void Import()
    {
        GeoCoordinate test = new GeoCoordinate();
        GeoCoordinate origin = new GeoCoordinate();
        origin.Latitude = (decimal)46.7302976970894;
        origin.Longitude = (decimal)-117.168948054314;
        CoordinateConverter myConverter = new CoordinateConverter();
        GeoCoordinate newPoint = new GeoCoordinate();
        MeterCoordinate result = new MeterCoordinate();
        MeterCoordinate originMeterPoint = new MeterCoordinate();
        originMeterPoint.X = (decimal)originPoint.transform.position.x;
        originMeterPoint.Y = (decimal)originPoint.transform.position.z;
        string line;
        int lineNum = 1;
        decimal[] nums = new decimal[20];
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
                nums[i] = decimal.Parse(word);
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
            result = myConverter.FindMeterCoordinateFromOrigin(originMeterPoint, origin, newPoint);
            /*Debug.Log("New point X = " + (float)result.X);
            Debug.Log("New point Y = " + (float)nums[10]);
            Debug.Log("New point Z = " + (float)result.Y);
            Debug.Log("New point final X = " + (originPoint.transform.position.x + (float)result.X));
            Debug.Log("New point final Y = " + (originPoint.transform.position.y + (float)nums[10]));
            Debug.Log("New point final Z = " + (originPoint.transform.position.z + (float)result.Y));*/
            GameObject point = Instantiate(masterPoint, new Vector3((float)result.X, originPoint.transform.position.y + (float)nums[10], (float)result.Y), Quaternion.identity);
            point.transform.name = "Sphere";
            point.AddComponent<LineRenderer>();
            Renderer newSphereRenderer = point.GetComponent(typeof(Renderer)) as Renderer;
            newSphereRenderer.enabled = true;
            points.Add(point);
            Debug.Log("");
            Array.Clear(words, 0, 11);
            lineNum++;

        }
        Debug.Log("Up and running");
    }
}
