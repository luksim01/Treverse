using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int bottlesCollected = 0;

    [Header("Environment")]
    [SerializeField] private GameObject environmentManager;

    [Header("Energy Connectors")]
    [SerializeField] private GameObject[] coalPlantConnectors;
    [SerializeField] private GameObject[] windFarmConnectors;

    void Start()
    {
        
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
}
