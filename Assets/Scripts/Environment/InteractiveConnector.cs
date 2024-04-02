using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDestruct
{
    public void AddObjectToDestroy(GameObject objectToDestroy);
}

interface ISlot
{
    public void ActivateSlot();
}

public class InteractiveConnector : MonoBehaviour, IInteractive
{
    Rigidbody gameObjectsRigidbody;
    [SerializeField] private float upwardForceMultiplier = 1f;
    [SerializeField] private float rotationalForceMultiplier = 1f;
    [SerializeField] private float upwardOffset = 1f;
    GameObject kayak;

    public GameObject cameraManager;
    private Interactor interactor;

    public ObjectStatus connectorStatus;

    private void Start()
    {
        kayak = GameObject.Find("Kayak");
        cameraManager = GameObject.Find("Camera Manager");
        interactor = cameraManager.GetComponent<Interactor>();
    }

    public void Interact()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObjectsRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        else
        {
            gameObjectsRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        // activate gravity
        gameObjectsRigidbody.isKinematic = false;

        // launch towards kayak
        Vector3 direction = kayak.transform.position - gameObject.transform.position;
        gameObjectsRigidbody.AddForce(new Vector3(direction.x, direction.y + upwardOffset, direction.z) * upwardForceMultiplier, ForceMode.Impulse);
        gameObjectsRigidbody.AddRelativeTorque(new Vector3(0.25f, 0.5f, 1f) * rotationalForceMultiplier);


        // activate buoyancy points
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // InsertConnector ActivateSlot HERE
        // need parent reference to activate slot
        Dictionary<GameObject, GameObject> connectorSlotPairs = interactor.GetConnectorSlotPairs();
        List<GameObject> connectorSlotPairRemoveList = new List<GameObject>();

        foreach (KeyValuePair<GameObject, GameObject> connectorSlotPair in connectorSlotPairs)
        {
            GameObject connector = connectorSlotPair.Key;
            GameObject slot = connectorSlotPair.Value;

            if (gameObject == connector)
            {
                slot.GetComponent<InsertConnector>().ActivateSlot();
                connectorSlotPairRemoveList.Add(connector);
            }
        }

        if(connectorSlotPairRemoveList.Count > 0)
        {
            interactor.RemoveConnectorSlotPair(connectorSlotPairRemoveList);
        }

        connectorSlotPairRemoveList.Clear();
    }

    public ObjectStatus GetObjectStatus()
    {
        return connectorStatus;
    }

    public void SetObjectStatus(ObjectStatus objectStatus)
    {
        connectorStatus = objectStatus;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == kayak)
        {
            connectorStatus = ObjectStatus.destroyed;
            kayak.GetComponentInChildren<Interactor>().AddObjectToDestroy(gameObject);
        }
    }

    void PrintDictionaryPairs(Dictionary<GameObject, GameObject> keyValuePairs)
    {
        foreach (KeyValuePair<GameObject, GameObject> keyValuePair in keyValuePairs)
        {
            Debug.LogFormat("Connector: {0} Slot: {1}", keyValuePair.Key, keyValuePair.Value);
        }
    }
}
