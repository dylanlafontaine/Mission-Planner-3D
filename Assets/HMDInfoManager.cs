using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!XRSettings.isDeviceActive) {
            Debug.Log("No Headset Active");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay")) {
            Debug.Log("Using Mock HMD");
        }
        else {
            Debug.Log("Headset " + XRSettings.loadedDeviceName + " is active");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
