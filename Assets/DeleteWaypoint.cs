using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteWaypoint : MonoBehaviour
{
    public Button myButton;
    private MasterController master;

    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
        master = (MasterController)FindObjectOfType(typeof(MasterController));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        master.points.Remove(spawnUI.selectedSphere.gameObject);
        Destroy(spawnUI.selectedSphere.gameObject);
        spawnUI.selectedSphere = null;
        spawnUI.onSphere = false;
    }
}
