using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractor
{
    public void AddConnectorSlotPair(GameObject connector, GameObject slot);
    public void RemoveConnectorSlotPair(List<GameObject> connectorSlotPairRemoveList);
    public Dictionary<GameObject, GameObject> GetConnectorSlotPairs();
}

public class InsertConnector : MonoBehaviour, ISlot
{
    public bool isConnectorTriggered = false;
    public bool isConnectorAdded = false;
    public GameObject connector;
    public GameObject cameraManager;
    public Interactor interactor;

    private void Start()
    {
        cameraManager = GameObject.Find("Camera Manager");
        interactor = cameraManager.GetComponent<Interactor>();
    }

    private void FixedUpdate()
    {
        if (!isConnectorAdded)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject connectorContact = transform.GetChild(i).gameObject;
                if (connectorContact.GetComponent<InteractiveSlot>().GetObjectStatus() == ObjectStatus.inactive)
                {
                    isConnectorTriggered = true;
                }
            }

            if (isConnectorTriggered)
            {                
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject connectorContact = transform.GetChild(i).gameObject;
                    connectorContact.GetComponent<InteractiveSlot>().SetObjectStatus(ObjectStatus.inactive);
                }

                isConnectorAdded = true;
                AddConnector();
                isConnectorTriggered = false;
            }
        }
    }

    void AddConnector()
    {
        GameObject connectorObjectInSlot = Instantiate(connector, transform.position, transform.rotation);
        interactor.AddConnectorSlotPair(connectorObjectInSlot, gameObject);
    }

    public void ActivateSlot()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject connectorContact = transform.GetChild(i).gameObject;
            connectorContact.GetComponent<InteractiveSlot>().SetObjectStatus(ObjectStatus.active);
            isConnectorAdded = false;
        }
    }
}
