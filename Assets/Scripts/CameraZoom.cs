using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera isoCamera;
    [SerializeField] float maxCameraDistance;
    [SerializeField] float minCameraDistance;
    float cameraDistance;
    [SerializeField] float sensitivity = 10f;
    CinemachineComponentBase componentBase;

    private void Update()
    {
        if (componentBase == null)
        {
            componentBase = isoCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            maxCameraDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;

            if (componentBase is CinemachineFramingTransposer)
            {
                if (cameraDistance < 0)
                {
                    (componentBase as CinemachineFramingTransposer).m_CameraDistance += cameraDistance;
                    if ((componentBase as CinemachineFramingTransposer).m_CameraDistance > maxCameraDistance)
                    {
                        (componentBase as CinemachineFramingTransposer).m_CameraDistance = maxCameraDistance;
                    }
                }
                else
                {
                    (componentBase as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
                    if ((componentBase as CinemachineFramingTransposer).m_CameraDistance < minCameraDistance)
                    {
                        (componentBase as CinemachineFramingTransposer).m_CameraDistance = maxCameraDistance;
                    }
                }
            }

        }
    }
}
