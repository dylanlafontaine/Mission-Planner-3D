using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

///<summary>
///help -- This class defines behavior for closing the Help display
///</summary>
public class Help : MonoBehaviour
{
    public Button myButton;
    public ImportExportPoints import;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");
        //When the app starts import becomes the script importExportPoints
        import = (ImportExportPoints)FindObjectOfType(typeof(ImportExportPoints));
        //Attaches an event listener to the Help button that calls the taskOnClick function
        myButton.onClick.AddListener(taskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        //If the user left clicks off of the Help display then continue
        if (Input.GetMouseButtonDown(0) && !CheckUIHover.BlockedByUI)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If the ray hit something continue
            if (Physics.Raycast(ray, out hit))
            {
                //If the ray hit something other than the Help display Panel continue
                if (hit.transform.name != "Panel")
                    //Turn off the display for the Help Panel
                    panel.SetActive(false);

            }
            else
            {
                //If the ray didn't hit anything turn off the display for the Help Panel
                panel.SetActive(false);
            }
        }
    }

    void taskOnClick()
    {
        //Sets the help PopUp component to true so it renders properly
        import.Help();
    }
}
