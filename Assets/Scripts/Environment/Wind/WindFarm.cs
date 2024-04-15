using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFarm : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        //WindFarmActive();
    }

    public void WindFarmActive()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject windTurbine = transform.GetChild(i).gameObject;
            GameObject windTurbineRotor = windTurbine.transform.GetChild(0).gameObject;
            windTurbineRotor.GetComponent<WindTurbine>().BeginRotating(rotationSpeed);
        }
    }

    public void WindFarmInactive()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject windTurbine = transform.GetChild(i).gameObject;
            GameObject windTurbineRotor = windTurbine.transform.GetChild(0).gameObject;
            windTurbineRotor.GetComponent<WindTurbine>().StopRotating();
        }
    }
}
