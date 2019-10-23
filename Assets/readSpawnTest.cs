using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readSpawnTest : MonoBehaviour
{
    GeoCoordinate test = new GeoCoordinate();
    public GameObject myPrefab;
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
            Instantiate(myPrefab, new Vector3(nums[1], nums[2], nums[3]), Quaternion.identity);
            //Debug.Log(line);
        }
        Debug.Log("Up and running");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
