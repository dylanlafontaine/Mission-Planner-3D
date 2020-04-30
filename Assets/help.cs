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
    public GameObject panel;
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
        if (Input.GetMouseButtonDown(0) && !CheckUIHover.BlockedByUI)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name != "Panel")
                    panel.SetActive(false);

            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    void taskOnClick()
    {
        import.Help();
    }
}
