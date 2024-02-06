using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayakController : MonoBehaviour
{
    public Rigidbody rigidBody;
    private float waterLevel;

    private GameObject waterPlane;
    private Vector3[] waterVertices;

    // inputs
    private float forwardInput;
    private float horizontalInput;

    // movement settings : kayak
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        waterPlane = GameObject.Find("Water Plane");
    }

    void Update()
    {
        MovementControl(forwardSpeed, rotationSpeed);
    }

    void FixedUpdate()
    {
        waterLevel = GetKayakWaterLevel();
        KayakBobbing(waterLevel);
    }

    private void MovementControl(float forwardSpeed, float rotationSpeed)
    {
        // forwards movement
        forwardInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * forwardInput * Time.deltaTime * forwardSpeed);

        // horizontal rotation
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * rotationSpeed);
    }

    void KayakBobbing(float waterLevel)
    {
        // kayak is pushed vertically by the water level
        if (transform.position.y < waterLevel)
        {
            float displacementMultiplier = waterLevel - transform.position.y;
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }

    float GetKayakWaterLevel()
    {
        // get the water level at the kayak position
        waterVertices = waterPlane.GetComponent<MakeSomeNoise>().vertices;
        float waterLevel = 1.5f;
        for (int i = 0; i < waterVertices.Length; i++)
        {
            // vertices transformed from local to world coordinates
            waterVertices[i] = waterPlane.transform.TransformPoint(waterVertices[i]);
            // allow for overlap between water areas based on water area scaling 
            if (transform.position.x < waterVertices[i].x + (waterPlane.transform.localScale.x) &&
               transform.position.x > waterVertices[i].x - (waterPlane.transform.localScale.x) &&
               transform.position.z < waterVertices[i].z + (waterPlane.transform.localScale.z) &&
               transform.position.z > waterVertices[i].z - (waterPlane.transform.localScale.z))
            {
                waterLevel = waterVertices[i].y + 1.0f;
            }
        }
        return waterLevel;
    }

    // update the water level dependind on water area kayak is in
    private void OnTriggerEnter(Collider other)
    {
        waterPlane = other.gameObject;
    }
}
