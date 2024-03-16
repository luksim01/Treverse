using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // camera option
    public bool POVCameraStatus = true;
    public bool droneCameraStatus = false;

    public GameObject virtualPOVCamera;
    public GameObject virtualDroneCamera;

    void Update()
    {
        CheckViewToggle();
    }

    private void CheckViewToggle()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            POVCameraStatus = !POVCameraStatus;
            virtualPOVCamera.SetActive(POVCameraStatus);

            droneCameraStatus = !droneCameraStatus;
            virtualDroneCamera.SetActive(droneCameraStatus);
        }

    }
}
