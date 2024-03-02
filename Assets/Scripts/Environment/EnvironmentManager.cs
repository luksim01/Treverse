using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    WaterManager waterManager;

    public enum WaterColour
    {
        shallow,
        deep,
        polluted,
        apocalyptic,
        black
    }

    public WaterColour waterColour;

    private Color shallowWaterColor = new Color(0.0f, 0.85f, 1.0f, 0.6f);
    private Color deepWaterColor = new Color(0.0f, 0.15f, 0.5f, 0.9f);
    private Color pollutedWaterColor = new Color(0.2f, 0.5f, 0.35f, 0.9f);
    private Color apocalypticWaterColor = new Color(0.75f, 0.0f, 0.0f, 0.95f);
    private Color blackWaterColor = new Color(0.0f, 0.0f, 0.0f, 0.97f);

    [SerializeField] float waterColourChangeSpeed;

    void Start()
    {
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
    }

    void Update()
    {
        ManageWaterColour();
    }

    private void ManageWaterColour()
    {
        switch (waterColour)
        {
            case WaterColour.shallow:
                waterManager.InitiateWaterTileColourChangeTo(shallowWaterColor, waterColourChangeSpeed);
                break;
            case WaterColour.deep:
                waterManager.InitiateWaterTileColourChangeTo(deepWaterColor, waterColourChangeSpeed);
                break;
            case WaterColour.polluted:
                waterManager.InitiateWaterTileColourChangeTo(pollutedWaterColor, waterColourChangeSpeed);
                break;
            case WaterColour.apocalyptic:
                waterManager.InitiateWaterTileColourChangeTo(apocalypticWaterColor, waterColourChangeSpeed);
                break;
            case WaterColour.black:
                waterManager.InitiateWaterTileColourChangeTo(blackWaterColor, waterColourChangeSpeed);
                break;
        }
    }
}
