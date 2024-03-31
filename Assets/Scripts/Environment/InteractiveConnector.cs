using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDestruct
{
    public void AddObjectToDestroy(GameObject objectToDestroy);
}

public class InteractiveConnector : MonoBehaviour, IInteractive
{
    Rigidbody gameObjectsRigidbody;
    [SerializeField] private float upwardForceMultiplier = 1f;
    [SerializeField] private float rotationalForceMultiplier = 1f;
    [SerializeField] private float upwardOffset = 1f;
    GameObject kayak;

    public ObjectStatus connectorStatus;

    private void Start()
    {
        kayak = GameObject.Find("Kayak");
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

        gameObjectsRigidbody.isKinematic = false;

        Vector3 direction = kayak.transform.position - gameObject.transform.position;
        gameObjectsRigidbody.AddForce(new Vector3(direction.x, direction.y + upwardOffset, direction.z) * upwardForceMultiplier, ForceMode.Impulse);
        gameObjectsRigidbody.AddRelativeTorque(new Vector3(0.25f, 0.5f, 1f) * rotationalForceMultiplier);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
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
            //Destroy(gameObject);
        }
    }
}
