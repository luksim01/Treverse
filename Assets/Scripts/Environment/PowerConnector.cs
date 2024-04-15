using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerConnector : MonoBehaviour
{
    public GameObject connectorHolder;
    public GameObject kayak;
    Rigidbody rigidBody;

    public bool connectorEquipped = false;

    private void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckForDrop();
    }

    void CheckForDrop()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DropConnector();
        }
    }

    void EquipConnector()
    {
        rigidBody.isKinematic = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.parent = kayak.transform;
        transform.position = connectorHolder.transform.position;
        connectorEquipped = true;
    }

    void DropConnector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        rigidBody.isKinematic = false;
        transform.parent = null;
        transform.position = new Vector3(kayak.transform.localPosition.x, kayak.transform.localPosition.y, kayak.transform.localPosition.z - 10);
        connectorEquipped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == kayak.name && !connectorEquipped)
        {
            EquipConnector();
        }
    }
}
