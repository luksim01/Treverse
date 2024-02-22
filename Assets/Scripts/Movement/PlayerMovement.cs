using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool inKayak = false;
    public GameObject kayakObject;
    public GameObject cameraPos;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Ground check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private float kayakHeightBoost = 0.9f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        kayakObject = GameObject.Find("Kayak");
    }

    void Update()
    {
        // ToDo: Adjust drag & moveSpeed based on if in water
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        rb.drag = grounded ? groundDrag : 0;

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnterExitKayak();
        }
    }

    void FixedUpdate()
    {
        if (!inKayak) {
            MovePlayer();
            SpeedControl();
        }
    }

    private void LateUpdate()
    {
        if (inKayak)
        {
            transform.position = kayakObject.transform.position;
            orientation.rotation = kayakObject.transform.rotation;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void EnterExitKayak ()
    {
        if (inKayak)
        {
            inKayak = false;
            GetComponentInChildren<CapsuleCollider>().enabled = true;
            kayakObject.GetComponent<KayakController>().hasPlayer = false;
            cameraPos.transform.position = new Vector3(cameraPos.transform.position.x, cameraPos.transform.position.y - kayakHeightBoost, cameraPos.transform.position.z);

        } else
        {
            float distanceToKayak = Vector3.Distance(transform.position, kayakObject.transform.position);
            if (distanceToKayak < 7)
            {
                inKayak = true;
                transform.position = kayakObject.transform.position;
                orientation.rotation = kayakObject.transform.rotation;
                GetComponentInChildren<CapsuleCollider>().enabled = false;

                kayakObject.GetComponent<KayakController>().hasPlayer = true;
                cameraPos.transform.position = new Vector3(cameraPos.transform.position.x, cameraPos.transform.position.y + kayakHeightBoost, cameraPos.transform.position.z);
            }
        }
    }
}
