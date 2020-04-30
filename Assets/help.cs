using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class help : MonoBehaviour
{
    public Button myButton;
    public importExportPoints import;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");
        import = (importExportPoints)FindObjectOfType(typeof(importExportPoints));
        myButton.onClick.AddListener(taskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void taskOnClick()
    {
        import.Help();
    }
}
