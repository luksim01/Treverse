using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBottle : MonoBehaviour, IInteractiveObject
{
    private Rigidbody gameObjectsRigidbody;
    [SerializeField] private float directionalForceMultiplier;
    [SerializeField] private float rotationalForceMultiplier;
    [SerializeField] private Vector3 directionOffset;
    [SerializeField] private Vector3 rotationOffset;

    private GameObject kayak;
    private GameObject cameraManager;
    private Interactor interactor;

    private InteractiveObjectStatus connectorStatus;

    GameObject parentObject;

    private void Start()
    {
        kayak = GameObject.Find("Kayak");
        cameraManager = GameObject.Find("Camera Manager");
        interactor = cameraManager.GetComponent<Interactor>();
        parentObject = gameObject.transform.parent.gameObject;
    }

    public void Interact()
    {
        // activate gravity
        gameObjectsRigidbody = parentObject.GetComponent<Rigidbody>();

        // launch towards kayak
        Vector3 direction = kayak.transform.position - parentObject.transform.position;
        gameObjectsRigidbody.AddForce((new Vector3(direction.x, direction.y, direction.z) + directionOffset) * directionalForceMultiplier, ForceMode.Impulse);
        gameObjectsRigidbody.AddRelativeTorque(rotationOffset * rotationalForceMultiplier);
    }

    public InteractiveObjectStatus GetObjectStatus()
    {
        return connectorStatus;
    }

    public void SetObjectStatus(InteractiveObjectStatus objectStatus)
    {
        connectorStatus = objectStatus;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == kayak)
        {
            connectorStatus = InteractiveObjectStatus.destroyed;
            interactor.AddObjectToDestroy(parentObject);
        }
    }
}
