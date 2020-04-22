using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text;

using System.IO;
using UnityEditor;


public class importExportPoints : MonoBehaviour
{
    public UnityEngine.UI.Button importButton;
    public UnityEngine.UI.Button exportButton;

    public GameObject masterPoint;
    public GameObject originPoint;
    GeoCoordinate origin;

    private Waypoints points;
    // Start is called before the first frame update
    void Start()
    {
        origin = new GeoCoordinate();
        origin.Latitude = (decimal)46.7302976970894;
        origin.Longitude = (decimal)-117.168948054314;

        importButton.onClick.AddListener(Import);
        exportButton.onClick.AddListener(Export);
        //points = ((MasterController)FindObjectOfType(typeof(MasterController))).points;

        points = (Waypoints)FindObjectOfType(typeof(Waypoints));
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void Import()
    {
        
        string line;
        int lineNum = 1;
        decimal[] nums = new decimal[20];
        string[] words = { };

        //this launches the windows file explorer
        string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");

        // convert string to System.IO.StreamReader for reading file  
        System.IO.StreamReader file =
            new System.IO.StreamReader(@path);

        //Ignoring header
        if ((line = file.ReadLine()) == null)
            Debug.Log("Blank File!!!");
        //Ignoring sealevel?
        if ((line = file.ReadLine()) == null)
            Debug.Log("Blank File!!!");

        print("Imported: ");

        while ((line = file.ReadLine()) != null)
        {
            words = Regex.Split(line, "\t");

            int i = 0;
            foreach (string word in words)
            {
                nums[i] = decimal.Parse(word);
                i++;
            }

            points.importWaypoint(
                (int)nums[0], // number
                (int)nums[2], // frame
                (int)nums[3], // command
                nums[4], // delay
                nums[5], // radius
                nums[6], // pass
                nums[7], // yaw
                (float)nums[8], // lat
                (float)nums[9], // lon
                (float)nums[10] //altitude
                );


            print(
                nums[0].ToString() + "\t" +// number
                nums[2].ToString() + "\t" + // frame
                nums[3].ToString() + "\t" + // command
                nums[4].ToString() + "\t" + // delay
                nums[5].ToString() + "\t" + // radius
                nums[6].ToString() + "\t" + // pass
                nums[7].ToString() + "\t" + // yaw
                nums[8].ToString() + "\t" + // lat
                nums[9].ToString() + "\t" + // lon
                nums[10].ToString() //altitude
                );
            /*//lat = nums[8]
            newPoint.Latitude = nums[8];
            //long = nums[9]
            newPoint.Longitude = nums[9];
            //height = nums[10]
            Debug.Log("Origin Lat = " + origin.Latitude + "\nnewPoint Lat = " + newPoint.Latitude);
            Debug.Log("Origin Long = " + origin.Longitude + "\nnewPoint Long = " + newPoint.Longitude);
            result = myConverter.FindMeterCoordinateFromOrigin(originMeterPoint, origin, newPoint);
            Debug.Log("New point X = " + (float)result.X);
            Debug.Log("New point Y = " + (float)nums[10]);
            Debug.Log("New point Z = " + (float)result.Y);
            Debug.Log("New point final X = " + (originPoint.transform.position.x + (float)result.X));
            Debug.Log("New point final Y = " + (originPoint.transform.position.y + (float)nums[10]));
            Debug.Log("New point final Z = " + (originPoint.transform.position.z + (float)result.Y));
            GameObject point = Instantiate(masterPoint, new Vector3((float)result.X, originPoint.transform.position.y + (float)nums[10], (float)result.Y), Quaternion.identity);
            point.transform.name = "Sphere";
            point.AddComponent<LineRenderer>();
            Renderer newSphereRenderer = point.GetComponent(typeof(Renderer)) as Renderer;
            newSphereRenderer.enabled = true;
            points.Add(point);
            Debug.Log("");*/
            Array.Clear(words, 0, 11);
            lineNum++;
        }
        file.Close();
        Debug.Log("Up and running");
        print("#points: " + points.points.Count);
    }

    public void Export()
    {
        // init variables
        string firstLine; // stores the first line
        string secondLine; // stores second line
        StringBuilder lineBuilder = new StringBuilder();
        int lineNum = 1; // keeps track of the line number
        string line;
        MeterCoordinate xy = new MeterCoordinate();
        decimal z;
        GeoCoordinate latlon = new GeoCoordinate();
        MeterCoordinate originMeter = new MeterCoordinate(Convert.ToDecimal(originPoint.transform.position.x), Convert.ToDecimal(originPoint.transform.position.z));
        CoordinateConverter converter = new CoordinateConverter();

        // infile/outfile
        System.IO.StreamReader inFile =
            new System.IO.StreamReader(@"SloanTest.waypoints");
        System.IO.StreamWriter outFile = new System.IO.StreamWriter(@"SloanTestOutput.waypoints");


        //Ignoring header
        if ((firstLine = inFile.ReadLine()) == null)
            print("Blank File!!!");

        //Ignoring sealevel?
        if ((secondLine = inFile.ReadLine()) == null)
            print("Blank File!!!");

        outFile.WriteLine(firstLine);
        outFile.WriteLine(secondLine);

        print("Exported:");
        foreach (Waypoint point in points.points)
        {
            outFile.WriteLine(point.export());
            print(point.export());
        }
        inFile.Close();
        outFile.Close();
    }
}
