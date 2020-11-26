using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtons : MonoBehaviour
{
    Ray ray;
    Vector3 point;
    float distance = 4.5F;
    public GameObject canvas;
    public GameObject addButton;
    GameObject newButton;
    // Start is called before the first frame update
    void Start()
    {
        newButton = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CheckUIHover.BlockedByUI)
        {
            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //point = ray.origin + new Vector3(ray.direction.x * distance, ray.direction.y * distance, ray.direction.z * distance);
            addButton.transform.position = new Vector3(Input.mousePosition.x + 50, Input.mousePosition.y, 0);
        }
    }
}
