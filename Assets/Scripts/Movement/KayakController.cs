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
    [SerializeField] private float boostMultiplier = 1f;
    [SerializeField] private bool isBoostActive = false;
    [SerializeField] private float boostDuration = 1f;

    // movement : floating
    public List<GameObject> waterTilesInContact;
    public float waterTileCenterToSideLengthX;
    public float waterTileCenterToSideLengthZ;

    // drone camera
    CameraManager cameraManager;
    bool isDroneCameraActive;
    [SerializeField] private float forwardSpeedDroneCamera;

    GameManager gameManager;

 /*   // Paddle Audio Generator
    [SerializeField] PaddleGenerator paddleGenerator;
    [SerializeField] private float paddleTimer;
   
  
    bool isPaddling = false;
   */


    void Start()
    {
        WaterManager waterManager;
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
        waterTileCenterToSideLengthX = (int)waterManager.GetTileLength().x / 2;
        waterTileCenterToSideLengthZ = (int)waterManager.GetTileLength().z / 2;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        /*  paddleGenerator = GetComponent<PaddleGenerator>();

          Debug.Assert(isPaddling, "Chris: Lukas, Line 116, wondering about a mini speed boost when the sound is played with a cool down time maybe? Thinking about the feeling of being in a kayak, the push & pull, probaly a thing for down the line!");
  */
    }


    void FixedUpdate()
    {
        isDroneCameraActive = cameraManager.droneCameraStatus;

        CheckForSpeedBoost();

        MovementControl(forwardSpeed, rotationSpeed, isDroneCameraActive);


      /*  // Audio Logic
        if (Input.GetKeyDown(KeyCode.W) && !isPaddling)
        {
            isPaddling = true;
            StartCoroutine(PlayPaddleClip(paddleTimer));
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            isPaddling = false;
            paddleGenerator.audioSource.Stop();
        }*/
    }

    private void CheckForSpeedBoost()
    {
        if (gameManager.isHoldingBottle() && !isBoostActive)
        {
            isBoostActive = true;
            StartCoroutine(BoostDurationCountdown());
        }
    }

    IEnumerator BoostDurationCountdown()
    {
        boostMultiplier = 2f;
        yield return new WaitForSeconds(boostDuration);
        gameManager.ChangeBottleCollectedBy(-1);
        if(gameManager.GetBottlesHeld() > 0)
        {
            StartCoroutine(BoostDurationCountdown());
        }
        else
        {
            isBoostActive = false;
            boostMultiplier = 1f;
        }
    }

    private void MovementControl(float forwardSpeed, float rotationSpeed, bool isDroneCameraActive)
    {
        // forwards movement
        forwardInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * forwardInput * Time.deltaTime * (isDroneCameraActive ? forwardSpeedDroneCamera : forwardSpeed) * boostMultiplier);

        // horizontal rotation
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * rotationSpeed * boostMultiplier);

       
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

   /* void PlayPaddleSound()
    {
        StartCoroutine(PlayPaddleClip(paddleTimer));
    }

    IEnumerator PlayPaddleClip(float timer)
    {
        while (isPaddling)
        {
            int randomIndex = Random.Range(0, 5);
            paddleGenerator.audioSource.clip = paddleGenerator.paddleSounds[randomIndex];
            
            paddleGenerator.audioSource.Play();
            
            yield return new WaitForSeconds(timer);

           
        }
    }
*/
  
    

}
