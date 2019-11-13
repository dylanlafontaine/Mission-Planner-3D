using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnUI : MonoBehaviour
{
    public Button deleteButton, moveButton;
    public bool deleteInScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        deleteInScreen = false;
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
                if (hit.transform.name == "Sphere")
                {
                    deleteInScreen = true;
                    Debug.Log("My object is clicked by mouse");
                    deleteButton.transform.position = new Vector3(Input.mousePosition.x - 50, Input.mousePosition.y, 0);
                    moveButton.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 50, 0);
                }
                else
                {
                    deleteButton.transform.position = new Vector3(-1000, 0, 0);
                    deleteInScreen = false;
                    moveButton.transform.position = new Vector3(-1000, 0, 0);
                }
            }
            else
            {
                deleteButton.transform.position = new Vector3(-1000, 0, 0);
                deleteInScreen = false;
                moveButton.transform.position = new Vector3(-1000, 0, 0);
            }
        }
    }
}
