using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    WaterManager waterManager;
    GameObject waterParent;

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

    public float desiredWaterLevel;
    [SerializeField] float waterLevelChangeSpeed = 0.5f;

    void Start()
    {
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
        waterParent = GameObject.Find("Water");
    }

    void Update()
    {
        ManageWaterColour();
        ManageWaterLevel();
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

    private void ManageWaterLevel()
    {
        float currentWaterLevel = waterParent.transform.position.y;
        if (desiredWaterLevel != currentWaterLevel)
        {
            float waterLevelDifference = desiredWaterLevel - currentWaterLevel;

            if (Mathf.Abs(waterLevelDifference) < waterLevelChangeSpeed * Time.deltaTime)
            {
                currentWaterLevel = desiredWaterLevel;
            } else
            {
                currentWaterLevel += Mathf.Sign(waterLevelDifference) * waterLevelChangeSpeed * Time.deltaTime;
            }

            waterParent.transform.position = new Vector3(waterParent.transform.position.x, currentWaterLevel, waterParent.transform.position.z);
        }
    }
}
