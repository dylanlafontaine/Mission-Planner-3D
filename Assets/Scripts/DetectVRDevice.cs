using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using OVR.OpenVR;

public class DetectVRDevice : MonoBehaviour
{
    private bool isDeviceActive;
    public Image headsetEnabledImage;
    public Canvas twoDCanvas;
    public OnlineMaps map;
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
                controller = OVRInput.GetConnectedControllers();
                if (controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.RTouch)
                {
                    Debug.Log("Tracking is working");
                    leftHandAnchor.gameObject.SetActive(true);
                    rightHandAnchor.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Tracking is lost");
                    leftHandAnchor.gameObject.SetActive(false);
                    rightHandAnchor.gameObject.SetActive(false);
                }
            }
        }
    }

    void HandleHMDAquired()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoader();
        OVRPlayerRig.gameObject.SetActive(true);
        map.gameObject.SetActive(false);
        twoDCanvas.gameObject.SetActive(false);
        headsetEnabledImage.gameObject.SetActive(true);
    }

    void HandleHMDLost()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        OVRPlayerRig.gameObject.SetActive(false);
        map.gameObject.SetActive(true);
        twoDCanvas.gameObject.SetActive(true);
        headsetEnabledImage.gameObject.SetActive(false);
    }

    void HandleHMDMounted()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoader();
        OVRPlayerRig.gameObject.SetActive(true);
        map.gameObject.SetActive(false);
        twoDCanvas.gameObject.SetActive(false);
        headsetEnabledImage.gameObject.SetActive(true);
    }

    void HandleHMDUnmounted()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        OVRPlayerRig.gameObject.SetActive(false);
        map.gameObject.SetActive(true);
        twoDCanvas.gameObject.SetActive(true);
        headsetEnabledImage.gameObject.SetActive(false);
    }
}
