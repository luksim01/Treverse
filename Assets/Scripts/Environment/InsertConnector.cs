using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertConnector : MonoBehaviour
{
    public bool isConnectorAdded = false;
    public GameObject connector;

    private void FixedUpdate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject connectorContact =  transform.GetChild(i).gameObject;
            if (connectorContact.GetComponent<InteractiveSlot>().GetObjectStatus() == ObjectStatus.inactive)
            {
                isConnectorAdded = true;
            }
        }

        if (isConnectorAdded)
        {
            AddConnector();
            isConnectorAdded = false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject connectorContact = transform.GetChild(i).gameObject;
            connectorContact.GetComponent<InteractiveSlot>().SetObjectStatus(ObjectStatus.active);
        }
    }

    void AddConnector()
    {
        Instantiate(connector, transform.position, transform.rotation);
    }
}
