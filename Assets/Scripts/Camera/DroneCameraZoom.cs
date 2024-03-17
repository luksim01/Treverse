using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DroneCameraZoom : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera droneCamera;
    [SerializeField] float maxCameraDistanceY;
    [SerializeField] float minCameraDistanceY;
    [SerializeField] float maxCameraDistanceZ;
    [SerializeField] float minCameraDistanceZ;
    [SerializeField] float cameraDistanceY;
    [SerializeField] float cameraDistanceZ;
    [SerializeField] float sensitivity = 1f;
    CinemachineComponentBase componentBase;

    CameraManager cameraManager;
    bool isDroneCameraActive;

    private void Start()
    {
        if (componentBase == null)
        {
            componentBase = droneCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            minCameraDistanceY = (componentBase as CinemachineTransposer).m_FollowOffset.y;
            maxCameraDistanceZ = (componentBase as CinemachineTransposer).m_FollowOffset.z;
        }

        cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
    }

    private void Update()
    {
        isDroneCameraActive = cameraManager.droneCameraStatus;
        if (isDroneCameraActive)
        {
            DroneCameraZooming();
        }
    }

    private void DroneCameraZooming()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistanceY += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            cameraDistanceZ = cameraDistanceY * maxCameraDistanceZ / minCameraDistanceY;

            if (componentBase is CinemachineTransposer)
            {
                (componentBase as CinemachineTransposer).m_FollowOffset.y = cameraDistanceY;
                (componentBase as CinemachineTransposer).m_FollowOffset.z = cameraDistanceZ;

                if (cameraDistanceY < minCameraDistanceY)
                {
                    (componentBase as CinemachineTransposer).m_FollowOffset.y = minCameraDistanceY;
                    cameraDistanceY = minCameraDistanceY;
                }
                else if (cameraDistanceY > maxCameraDistanceY)
                {
                    (componentBase as CinemachineTransposer).m_FollowOffset.y = maxCameraDistanceY;
                    cameraDistanceY = maxCameraDistanceY;
                }

                if (cameraDistanceZ < minCameraDistanceZ)
                {
                    (componentBase as CinemachineTransposer).m_FollowOffset.z = minCameraDistanceZ;
                    cameraDistanceZ = minCameraDistanceZ;
                }
                else if (cameraDistanceZ > maxCameraDistanceZ)
                {
                    (componentBase as CinemachineTransposer).m_FollowOffset.z = maxCameraDistanceZ;
                    cameraDistanceZ = maxCameraDistanceZ;
                }
            }
        }
    }
}
