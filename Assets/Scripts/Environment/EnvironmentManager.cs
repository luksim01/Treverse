using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnvironmentManager : MonoBehaviour
{
    public enum WaterColour
    {
        shallow,
        deep,
        polluted,
        apocalyptic,
        black
    }
    WaterManager waterManager;
    GameObject waterParent;


    [Header("Water Colour")]
    public WaterColour waterColour;

    private Color shallowWaterColor = new Color(0.0f, 0.85f, 1.0f, 0.6f);
    private Color deepWaterColor = new Color(0.0f, 0.15f, 0.5f, 0.9f);
    private Color pollutedWaterColor = new Color(0.2f, 0.5f, 0.35f, 0.9f);
    private Color apocalypticWaterColor = new Color(0.75f, 0.0f, 0.0f, 0.95f);
    private Color blackWaterColor = new Color(0.0f, 0.0f, 0.0f, 0.97f);

    [SerializeField] float waterColourChangeSpeed;

    [Header("Water Level")]
    public float desiredWaterLevel;
    [SerializeField] float waterLevelChangeSpeed = 0.5f;


    [Header("Power Station")]
    public bool powerStationActive = true;
    [SerializeField] GameObject powerStationSmoke;
    [SerializeField] GameObject powerStationFog;

    private float shrinkSpeed = 0.05f;

    void Start()
    {
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
        waterParent = GameObject.Find("Water");
        Debug.Assert(waterParent, "Lukas: Nick, line 47, there is no Water parent anymore, can you update the code with the latest scene hierarchy changes");

        Debug.Assert(powerStationSmoke, "Lukas: Nick, line 39, I have deactivate fog while fixing the scene, can you revisit and make sure all is OK with this code");
        Debug.Assert(powerStationFog, "Lukas: Nick, line 40, I have deactivate fog while fixing the scene, can you revisit and make sure all is OK with this code");
    }

    void Update()
    {
        ManageWaterColour();
        if(waterParent != null)
        {
            ManageWaterLevel();
        }

        if (powerStationSmoke != null && powerStationFog != null)
        {
            ManagePowerStation();
        }
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

    private void ManagePowerStation()
    {
        Vector3 smokeScale = powerStationSmoke.transform.localScale;
        if (!powerStationActive)
        {
            if (powerStationSmoke.activeSelf)
            {
                float shrinkAmount = shrinkSpeed * Time.deltaTime;
                powerStationSmoke.transform.localScale =
                    new Vector3(smokeScale.x - shrinkAmount, smokeScale.y - shrinkAmount, smokeScale.z - shrinkAmount);

                if (smokeScale.x < 0)
                {
                    powerStationSmoke.SetActive(false);
                }

            }
            if (powerStationFog.activeSelf)
            {
                LocalVolumetricFog fog = powerStationFog.GetComponent<LocalVolumetricFog>();
                float fogDistance = fog.parameters.meanFreePath;
                if (fogDistance < 1000)
                {
                    fog.parameters.meanFreePath += 2 * Time.deltaTime;
                }
                else
                {
                    powerStationFog.SetActive(false);
                }
            }
        } else if (smokeScale.x < 1)
        {
            powerStationSmoke.SetActive(true);
            float growAmount = shrinkSpeed * Time.deltaTime;
            powerStationSmoke.transform.localScale =
                new Vector3(smokeScale.x + growAmount, smokeScale.y + growAmount, smokeScale.z + growAmount);
        }
    }
}
