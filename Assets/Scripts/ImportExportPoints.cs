﻿using System;
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

///<summary>
///importExportPoints -- Handles the behavior for importing waypoints from a file and for exporting waypoints to a file
///</summary>
public class ImportExportPoints : MonoBehaviour
{
    public UnityEngine.UI.Button importButton;
    public UnityEngine.UI.Button exportButton;

    public GameObject masterPoint;
    public GameObject originPoint;
    GeoCoordinate origin;

    private Waypoints points;
	
	
	//Help button
	public UnityEngine.UI.Button HelpButton;
	public bool toggle = false;
	//public UnityEngine.UI.Button Exit;
	public GameObject PopUp;
	//public GameObject Background;
	
	///<summary>
    ///Start -- Is called before the first frame update
    ///</summary>
    void Start()
    {
        //Creates a new Online Maps v3 GeoCoordinate object
        origin = new GeoCoordinate();
        //Sets the origin to latitude of 46.7 and longitude to -117.2 which is Pullman, Washington
        origin.Latitude = (decimal)46.7302976970894;
        origin.Longitude = (decimal)-117.168948054314;

        //Adds event listeners to the Import and Export button
        importButton.onClick.AddListener(Import);
        exportButton.onClick.AddListener(Export);
        //points = ((MasterController)FindObjectOfType(typeof(MasterController))).points;

        //Sets points to the Waypoints object 
        points = (Waypoints)FindObjectOfType(typeof(Waypoints));
		
		
		//help functions
		PopUp.SetActive(true);
		//Background.SetActive(false);
		HelpButton.onClick.AddListener(Help);
		//Exit.onClick.AddListener(Leave);
    }
	
    ///<summary>
	///Help -- Help functions that aren't used
    ///</summary>
	public void Help()
	{
        PopUp.SetActive(true);
	}

    ///<summary>
    ///Import -- Handles the importation of waypoints from a file
    ///</summary>
    public void Import()
    {
        
        string line;
        int lineNum = 1;
        decimal[] nums = new decimal[20];
        string[] words = { };

        //this launches the windows file explorer
        string path = EditorUtility.OpenFilePanel("", "", "");

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

        //Imports waypoints from the file line by line 
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
                (float)nums[8], // long
                (float)nums[9], // lat
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
                nums[8].ToString() + "\t" + // long
                nums[9].ToString() + "\t" + // lat
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

    ///<summary>
    ///NOTE Dylan L. -- This function isn't set up to work properly yet
    ///Export -- Will Export the waypoints to a telemetry file
    ///</summary>
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
