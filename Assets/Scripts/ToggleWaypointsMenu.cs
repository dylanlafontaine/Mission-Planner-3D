﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWaypointsMenu : MonoBehaviour
{
    public GameObject waypointsMenu;
    public Button toggleButton;
    public Scrollbar scrollbar;

    public void HandleMenuToggle()
    {

        if (waypointsMenu.activeSelf == true)
        {
            waypointsMenu.SetActive(false);
            toggleButton.image.transform.Rotate(0, 0, 180);
            scrollbar.gameObject.SetActive(false);
        }
        else
        {
            waypointsMenu.SetActive(true);
            toggleButton.image.transform.Rotate(0, 0, 180);
            scrollbar.gameObject.SetActive(true);
        }
    }
}
