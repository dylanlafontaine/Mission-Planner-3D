using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FocusSphere : MonoBehaviour
{
    private Camera cam;
    public Dropdown myDropdown;
    public GameObject testSphere;

    // Start is called before the first frame update
    void Start()
    {
        myDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(myDropdown);
        });
        cam = Camera.main;
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }

    void DropdownValueChanged(Dropdown change)
    {
        Debug.Log(change);
        cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, new Vector3(testSphere.transform.localPosition.x, testSphere.transform.localPosition.y, testSphere.transform.localPosition.z - 20), 100);
    }
}
