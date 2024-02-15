using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayakController : MonoBehaviour
{
    public Rigidbody rigidBody;

    // inputs
    private float forwardInput;
    private float horizontalInput;

    // movement settings : kayak
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotationSpeed;

    // movement : floating
    public List<GameObject> waterTilesInContact;
    public float waterTileCenterToSideLengthX;
    public float waterTileCenterToSideLengthZ;

    void Start()
    {
        WaterManager waterManager;
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
        waterTileCenterToSideLengthX = (int)waterManager.GetTileLength().x / 2;
        waterTileCenterToSideLengthZ = (int)waterManager.GetTileLength().z / 2;
    }

    void Update()
    {
        MovementControl(forwardSpeed, rotationSpeed);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WaterTile"))
        {
            GameObject waterTile = other.gameObject;
            if (!waterTilesInContact.Contains(waterTile))
            {
                waterTilesInContact.Add(waterTile);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WaterTile"))
        {
            GameObject waterTile = other.gameObject;
            waterTilesInContact.Remove(waterTile);
        }
    }
}
