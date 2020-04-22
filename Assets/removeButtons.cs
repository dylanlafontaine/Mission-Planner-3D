using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class removeButtons : MonoBehaviour
{
    public Button add, delete, up, down;
    public Renderer sphereRender;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!spawnUI.onSphere)
        {
            add.transform.position = new Vector3(-1000, 0, 0);
            delete.transform.position = new Vector3(-1000, 0, 0);
            up.transform.position = new Vector3(-1000, 0, 0);
            down.transform.position = new Vector3(-1000, 0, 0);
            if (spawnUI.selectedSphere != null)
            {
                sphereRender = spawnUI.selectedSphere.GetComponent(typeof(Renderer)) as Renderer;
                sphereRender.material.color = Color.white;
            }
        }*/
    }
}
