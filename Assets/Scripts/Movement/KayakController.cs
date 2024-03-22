using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // drone camera
    CameraManager cameraManager;
    bool isDroneCameraActive;
    [SerializeField] private float forwardSpeedDroneCamera;

    // [SerializeField] PaddleGenerator paddleGenerator;
     [SerializeField] private FMODUnity.EventReference paddleAudio;
     private FMOD.Studio.EventInstance paddleLoop;

    bool isPaddling = false;
   

    void Start()
    {
        WaterManager waterManager;
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
        waterTileCenterToSideLengthX = (int)waterManager.GetTileLength().x / 2;
        waterTileCenterToSideLengthZ = (int)waterManager.GetTileLength().z / 2;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();

        // paddleGenerator = GetComponent<PaddleGenerator>();

        paddleLoop = RuntimeManager.CreateInstance(paddleAudio);
        

        Debug.Assert(isPaddling, "Chris: Lukas, Line 116, wondering about a mini speed boost when the sound is played with a cool down time maybe? Thinking about the feeling of being in a kayak, the push & pull, probaly a thing for down the line!");

    }

    private void Update()
    {
  
        // Audio Logic
        if (Input.GetKeyDown(KeyCode.W) && !isPaddling)
        {
            isPaddling = true;
            paddleLoop.start();
            Debug.Log("Playing");


        }
        else if (Input.GetKeyUp(KeyCode.W) && isPaddling)
        {
            isPaddling = false;
            paddleLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Debug.Log("Stopping");
        }
    }

    void FixedUpdate()
    {
        // read that FixedUpdate and Input don't play nicely.

        isDroneCameraActive = cameraManager.droneCameraStatus;

        MovementControl(forwardSpeed, rotationSpeed, isDroneCameraActive);

        paddleLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(paddleLoop, GetComponent<Transform>(), GetComponent<Rigidbody>());

    }

    private void MovementControl(float forwardSpeed, float rotationSpeed, bool isDroneCameraActive)
    {
        // forwards movement
        forwardInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * forwardInput * Time.deltaTime * (isDroneCameraActive ? forwardSpeedDroneCamera : forwardSpeed));

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
