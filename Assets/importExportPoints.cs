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
    public List<GameObject> points;
    
    public UnityEngine.UI.Button importButton;
    public UnityEngine.UI.Button exportButton;

    public GameObject masterPoint;
    public GameObject originPoint;
    GeoCoordinate origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = new GeoCoordinate();
        origin.Latitude = (decimal)46.7302976970894;
        origin.Longitude = (decimal)-117.168948054314;

        importButton.onClick.AddListener(Import);
        exportButton.onClick.AddListener(Export);
        points = ((MasterController)FindObjectOfType(typeof(MasterController))).points;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void Import()
    {
        
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
        file.Close();
        Debug.Log("Up and running");
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
            Debug.Log("Blank File!!!");

        //Ignoring sealevel?
        if ((secondLine = inFile.ReadLine()) == null)
            Debug.Log("Blank File!!!");

        outFile.WriteLine(firstLine);
        outFile.WriteLine(secondLine);

        string[] words = { };
        line = inFile.ReadLine();
        words = Regex.Split(line, "\t");

        foreach (GameObject point in points)
        {
            lineBuilder.Clear();

            // get x y and z
            xy.X = Convert.ToDecimal(point.transform.position.x);
            xy.Y = Convert.ToDecimal(point.transform.position.z);
            z = Convert.ToDecimal(point.transform.position.y);

            // get lat lon
            latlon = converter.MeterCoordtoGeoCoord(origin, originMeter, xy);

            lineBuilder.Append(lineNum.ToString() + "\t");
            lineBuilder.Append(words[1] + "\t" + words[2] + "\t" + words[3] + "\t" + words[4] + 
                "\t" + words[5] + "\t" + words[6] + "\t" + words[7] + "\t");

            lineBuilder.Append(((double)latlon.Latitude).ToString() + "\t");
            lineBuilder.Append(((double)latlon.Longitude).ToString() + "\t");
            lineBuilder.Append(((double)z).ToString() + "\t");  

            lineBuilder.Append(1.ToString());

            outFile.WriteLine(lineBuilder.ToString());

            lineNum++;
            
        }
        inFile.Close();
        outFile.Close();
    }
}
