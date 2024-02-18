using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public bool invertY = false;
    public float sensX;
    public float sensY;
    public Transform orientation;
    public GameObject playerObject;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // ToDo: Add reticle
    }

    // Update is called once per frame
    void Update()
    {
        // Player Input

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY * (invertY ? -1 : 1);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate camera and player orientation
        GameObject kayak = playerObject.GetComponent<PlayerMovement>().kayakObject;
        bool inKayak = playerObject.GetComponent<PlayerMovement>().inKayak;

        if (inKayak)
        {
            transform.rotation = Quaternion.Euler(kayak.transform.eulerAngles.x + xRotation, kayak.transform.eulerAngles.y + yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, kayak.transform.eulerAngles.y, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
