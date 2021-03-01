using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSphere : MonoBehaviour
{
    public float mainSpeed = 100.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.25f; //How sensitive it with mouse
    public bool rotateOnlyIfMousedown = true;
    public bool movementStaysFlat = true;
    public static Transform mySphere;
    public Vector3 baseVector;
    public Button add, delete, up, down;
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (SpawnUI.onSphere)
        {
            if (mySphere != null)
            {
                //baseVector = GetBaseInput();
                //baseVector = Camera.main.transform.TransformDirection(baseVector);
                //mySphere.Translate(baseVector * 1);
                baseVector = GetBaseInput();
                mySphere.Translate(baseVector);
                //delete.transform.Translate(new Vector3(baseVector.x, baseVector.y, 0));
                //add.transform.Translate(new Vector3(baseVector.x, baseVector.y, 0));
                //up.transform.Translate(new Vector3(baseVector.x, baseVector.y, 0));
                //down.transform.Translate(new Vector3(baseVector.x, baseVector.y, 0));
            }
        }
    }

    public Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || 
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            add.transform.position = new Vector3(-1000, 0, 0);
            delete.transform.position = new Vector3(-1000, 0, 0);
            up.transform.position = new Vector3(-1000, 0, 0);
            down.transform.position = new Vector3(-1000, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
            //p_Velocity = Camera.main.transform.TransformDirection(p_Velocity);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        return p_Velocity;
    }
}
