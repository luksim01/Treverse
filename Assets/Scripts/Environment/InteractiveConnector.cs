using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveConnector : MonoBehaviour, IInteractiveObject
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

    private void Start()
    {
        kayak = GameObject.Find("Kayak");
        cameraManager = GameObject.Find("Camera Manager");
        interactor = cameraManager.GetComponent<Interactor>();
    }

    public void Interact()
    {
        // activate gravity
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObjectsRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        else
        {
            gameObjectsRigidbody = gameObject.AddComponent<Rigidbody>();
        }
        gameObjectsRigidbody.isKinematic = false;

        // launch towards kayak
        Vector3 direction = kayak.transform.position - gameObject.transform.position;
        gameObjectsRigidbody.AddForce((new Vector3(direction.x, direction.y, direction.z) + directionOffset) * directionalForceMultiplier, ForceMode.Impulse);
        gameObjectsRigidbody.AddRelativeTorque(rotationOffset * rotationalForceMultiplier);

        // activate buoyancy points
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // check if interactive connector has an associated interactive slot to reactivate
        Dictionary<GameObject, GameObject> connectorSlotPairs = interactor.GetConnectorSlotPairs();
        List<GameObject> connectorSlotPairRemoveList = new List<GameObject>();

        foreach (KeyValuePair<GameObject, GameObject> connectorSlotPair in connectorSlotPairs)
        {
            GameObject connector = connectorSlotPair.Key;
            GameObject slot = connectorSlotPair.Value;

            if (gameObject == connector)
            {
                slot.GetComponent<Slot>().Reactivate();
                connectorSlotPairRemoveList.Add(connector);
            }
        }

        if(connectorSlotPairRemoveList.Count > 0)
        {
            interactor.RemoveConnectorSlotPair(connectorSlotPairRemoveList);
        }

        connectorSlotPairRemoveList.Clear();
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
            interactor.AddObjectToDestroy(gameObject);
        }
    }
}
