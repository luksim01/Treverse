using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActivateBeacon : MonoBehaviour
{
    public GameObject beaconObject;  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(beaconObject);
        }

    }


}
