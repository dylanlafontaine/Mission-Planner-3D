using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using OVR.OpenVR;

public class DetectVRDeviceVR : MonoBehaviour
{
    public Image enableVRImage;
    public GameObject citySimulatorMap;
    public GameObject OVRPlayerRig;
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    private float initialWait;
    private bool handsInitialized = false;
    private OVRInput.Controller controller;

    void Start()
    {
        OVRManager.HMDAcquired += HandleHMDAquired;
        OVRManager.HMDLost += HandleHMDLost;
        OVRManager.HMDMounted += HandleHMDMounted;
        OVRManager.HMDUnmounted += HandleHMDUnmounted;
        if (OVRManager.isHmdPresent)
        {
            HandleHMDAquired();
        }
        else
        {
            HandleHMDLost();
        }
    }

    void Update()
    {
        if (!handsInitialized)
        {
            initialWait += Time.deltaTime;
            if (initialWait > 1f)
            {
                controller = OVRInput.GetActiveController();
                if (controller != OVRInput.Controller.None)
                {
                    Debug.Log("Tracking is working");
                    leftHandAnchor.gameObject.SetActive(true);
                    rightHandAnchor.gameObject.SetActive(true);
                    handsInitialized = true;
                }
                else
                {
                    Debug.Log("Tracking is lost");
                    leftHandAnchor.gameObject.SetActive(false);
                    rightHandAnchor.gameObject.SetActive(false);
                    handsInitialized = true;
                }
            }
        }
    }

    void HandleHMDAquired()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoader();
        enableVRImage.gameObject.SetActive(false);
        citySimulatorMap.gameObject.SetActive(true);
        OVRPlayerRig.gameObject.SetActive(true);
    }

    void HandleHMDLost()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        enableVRImage.gameObject.SetActive(true);
        citySimulatorMap.gameObject.SetActive(false);
        OVRPlayerRig.gameObject.SetActive(false);
    }

    void HandleHMDMounted()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoader();
        enableVRImage.gameObject.SetActive(false);
        citySimulatorMap.gameObject.SetActive(true);
        OVRPlayerRig.gameObject.SetActive(true);
    }

    void HandleHMDUnmounted()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        enableVRImage.gameObject.SetActive(true);
        citySimulatorMap.gameObject.SetActive(false);
        OVRPlayerRig.gameObject.SetActive(false);
    }
}
