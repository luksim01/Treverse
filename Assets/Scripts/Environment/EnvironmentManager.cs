using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    WaterManager waterManager;

    private Color shallowWaterColor = new Color(0.0f, 0.85f, 1.0f, 0.6f);
    private Color deepWaterColor = new Color(0.0f, 0.15f, 0.5f, 0.9f);
    private Color pollutedWaterColor = new Color(0.2f, 0.5f, 0.35f, 0.9f);
    private Color apocalypticWaterColor = new Color(0.75f, 0.0f, 0.0f, 0.95f);
    private Color blackWaterColor = new Color(0.0f, 0.0f, 0.0f, 0.97f);

    [SerializeField] private bool isShallowWater = false;
    [SerializeField] private bool isDeepWater = false;
    [SerializeField] private bool isPollutedWater = false;
    [SerializeField] private bool isApocalypticWater = false;
    [SerializeField] private bool isBlackWater = false;

    [SerializeField] float waterColourChangeSpeed;

    void Start()
    {
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
    }

    void Update()
    {
        CheckForEnvironmentalChanges();
    }

    private void CheckForEnvironmentalChanges()
    {
        if (isShallowWater)
        {
            isShallowWater = waterManager.InitiateWaterTileColourChangeTo(shallowWaterColor, waterColourChangeSpeed);
        }
        else if (isDeepWater)
        {
            isDeepWater = waterManager.InitiateWaterTileColourChangeTo(deepWaterColor, waterColourChangeSpeed);
        }
        else if (isPollutedWater)
        {
            isPollutedWater = waterManager.InitiateWaterTileColourChangeTo(pollutedWaterColor, waterColourChangeSpeed);
        }
        else if (isApocalypticWater)
        {
            isApocalypticWater = waterManager.InitiateWaterTileColourChangeTo(apocalypticWaterColor, waterColourChangeSpeed);
        }
        else if (isBlackWater)
        {
            isBlackWater = waterManager.InitiateWaterTileColourChangeTo(blackWaterColor, waterColourChangeSpeed);
        }
    }
}
