using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int bottlesCollected = 0;

    [Header("Environment")]
    [SerializeField] private EnvironmentManager environmentManager;

    [Header("Energy Connectors")]
    [SerializeField] private GameObject[] coalPlantConnectors;
    [SerializeField] private GameObject[] windFarmConnectors;

    [SerializeField] private GameObject lightBeaconCoal;
    [SerializeField] private GameObject lightBeaconWind;
    [SerializeField] private GameObject lightBeaconCityHall;

    WindFarm windFarm;

    void Start()
    {
        windFarm = GameObject.Find("WindFarm").GetComponent<WindFarm>();
    }


    void Update()
    {

    }
        
    public int GetBottlesHeld()
    {
        return bottlesCollected;
    }

    public void ChangeBottleCollectedBy(int bottleQuantity)
    {
        bottlesCollected += bottleQuantity;
    }

    public bool isHoldingBottle()
    {
        if(bottlesCollected > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateEnvironment()
    {
        int activeCoalConnectors = 0;
        foreach (var connector in coalPlantConnectors)
        {
            if (connector.GetComponent<Slot>().isConnectorInSlot)
            {
                activeCoalConnectors++;
            }
        }

        int activeWindConnectors = 0;
        foreach (var connector in windFarmConnectors)
        {
            if (connector.GetComponent<Slot>().isConnectorInSlot)
            {
                activeWindConnectors++;
            }
        }
        if (activeCoalConnectors != 0)
        {
            // light beacon on coal
            lightBeaconCoal.SetActive(true);
            lightBeaconWind.SetActive(false);
            lightBeaconCityHall.SetActive(false);
            // keep wind farm inactive
            windFarm.WindFarmInactive();
            environmentManager.powerStationActive = true;
            // set environment to 100% pollution
            environmentManager.waterColour = EnvironmentManager.WaterColour.purpleApocalypse;
            environmentManager.fogIntensity = 0.5f;
        }
        else if (activeWindConnectors != windFarmConnectors.Length)
        {
            // light beacon on wind
            lightBeaconCoal.SetActive(false);
            lightBeaconWind.SetActive(true);
            lightBeaconCityHall.SetActive(false);

            // keep wind farm inactive
            windFarm.WindFarmInactive();
            environmentManager.powerStationActive = false;

            // set to 50% polluted
            environmentManager.waterColour = EnvironmentManager.WaterColour.polluted;
            environmentManager.fogIntensity = 0.1f;
        }
        else
        {
            // light beacon on city hall
            lightBeaconCoal.SetActive(false);
            lightBeaconWind.SetActive(false);
            lightBeaconCityHall.SetActive(true);

            // activate wind farm
            windFarm.WindFarmActive();
            environmentManager.powerStationActive = false;

            // set to clean environment
            environmentManager.waterColour = EnvironmentManager.WaterColour.shallow;
            environmentManager.fogIntensity = 0;
        }
    }
}
