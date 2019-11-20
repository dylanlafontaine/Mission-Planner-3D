using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveUp : MonoBehaviour
{
    public Button up, down;
    public List<GameObject> points;
    public GameObject selectedSphere;

    // Start is called before the first frame update
    void Start()
    {
        up.onClick.AddListener(moveUp);
        down.onClick.AddListener(moveDown);
        points = ((MasterController)FindObjectOfType(typeof(MasterController))).points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void moveUp()
    {
        int index;
        GameObject temp;
        selectedSphere = spawnUI.selectedSphere.gameObject;
        index = points.IndexOf(selectedSphere);
        if (index < points.Count - 1)
        {
            temp = points[index];
            points[index] = points[index + 1];
            points[index + 1] = temp;
        }
    }

    void moveDown()
    {
        int index;
        GameObject temp;
        selectedSphere = spawnUI.selectedSphere.gameObject;
        index = points.IndexOf(selectedSphere);
        if (index > 0)
        {
            temp = points[index];
            points[index] = points[index - 1];
            points[index - 1] = temp;
        }
    }
}
