using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerConnector : MonoBehaviour
{
    public GameObject connectorHolder;
    public GameObject kayak;

    public bool connectorEquipped = false;

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
        //for(int i =0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActiveRecursively(false);
        //}
        transform.parent = kayak.transform;
        transform.position = connectorHolder.transform.position;
        connectorEquipped = true;
    }

    void DropConnector()
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActiveRecursively(true);
        //}
        transform.parent = null;
        transform.position = new Vector3(kayak.transform.position.x, kayak.transform.position.y, kayak.transform.position.z - 10);
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
