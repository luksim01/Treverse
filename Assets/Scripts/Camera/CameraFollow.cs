using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraPosition;

    void LateUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
