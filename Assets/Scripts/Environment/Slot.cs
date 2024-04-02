using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInsertConnector
{
    public void Reactivate();
}

public class Slot : MonoBehaviour, IInsertConnector
{
    public bool isConnectorInSlot = false;
    private bool isSlotTriggered = false;

    public GameObject connector;
    private GameObject cameraManager;
    private Interactor interactor;

    private void Start()
    {
        cameraManager = GameObject.Find("Camera Manager");
        interactor = cameraManager.GetComponent<Interactor>();
    }

    private void FixedUpdate()
    {
        if (!isConnectorInSlot)
        {
            // check for interaction with any child objects while slot if empty
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject connectorContact = transform.GetChild(i).gameObject;
                if (connectorContact.GetComponent<InteractiveSlotElement>().GetObjectStatus() == InteractiveObjectStatus.inactive)
                {
                    isSlotTriggered = true;
                }
            }

            if (isSlotTriggered)
            {                
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject connectorContact = transform.GetChild(i).gameObject;
                    connectorContact.GetComponent<InteractiveSlotElement>().SetObjectStatus(InteractiveObjectStatus.inactive);
                }

                isConnectorInSlot = true;
                AddConnectorInSlot();
                isSlotTriggered = false;
            }
        }
    }

    void AddConnectorInSlot()
    {
        GameObject connectorObjectInSlot = Instantiate(connector, transform.position, transform.rotation);
        interactor.AddConnectorSlotPair(connectorObjectInSlot, gameObject);
    }

    // interface methods 
    public void Reactivate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject connectorContact = transform.GetChild(i).gameObject;
            connectorContact.GetComponent<InteractiveSlotElement>().SetObjectStatus(InteractiveObjectStatus.active);
            isConnectorInSlot = false;
        }
    }
}
