using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    bool isRotating = false;
    float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.back, speed * Time.deltaTime);
        }
    }

    public void BeginRotating(float rotationSpeed)
    {
        isRotating = true;
        speed = rotationSpeed;
    }

    public void StopRotating()
    {
        isRotating = false;
        speed = 0f;
    }
}
