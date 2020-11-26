using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckUIHover : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public static bool BlockedByUI;
    private EventTrigger eventTrigger;

    private void Start()
    {
        BlockedByUI = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit\n");
        CheckUIHover.BlockedByUI = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter\n");
        CheckUIHover.BlockedByUI = true;
    }
}