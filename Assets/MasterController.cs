using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    // replace with Waypoints object
    public List<GameObject> points = new List<GameObject>();
    Waypoints waypoints = new Waypoints();

    // Start is called before the first frame update
    void Start()
    {
        //GameObject map = GetComponent<GameObject>("Map");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
