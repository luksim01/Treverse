using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] Light directionalLight;
    public float fogIntensity = 0.7f; // 0 -> 1
    [SerializeField] GameObject localFogParent;
    [SerializeField] private float fogChangeSpeed = 15.0f;
    private LocalVolumetricFog localFog;

    public enum WaterColour
    {
        shallow,
        deep,
        polluted,
        apocalyptic,
        purpleApocalypse,
        black
    }
    WaterManager waterManager;


    [Header("Water Colour")]
    public WaterColour waterColour;

    private Color shallowWaterColor = new Color(0.0f, 0.85f, 1.0f, 0.6f);
    private Color deepWaterColor = new Color(0.0f, 0.15f, 0.5f, 0.9f);
    private Color pollutedWaterColor = new Color(0.2f, 0.5f, 0.35f, 0.9f);
    private Color apocalypticWaterColor = new Color(0.75f, 0.0f, 0.0f, 0.95f);
    private Color purpleApocalypseColor = new Color(1, 0.57f, 0.96f);
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
        localFog = localFogParent.GetComponent<LocalVolumetricFog>();
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
    }

    void Update()
    {
        if (localFog != null)
        {
            ManageLocalFog();
        } else
        {
            Debug.LogError("No local fog set!");
        }
        if(waterManager != null)
        {
            // ManageWaterLevel();
            // Temporarily removing until water tile bug is fixed
            ManageWaterColour();
        }

        if (powerStationSmoke != null)// && powerStationFog != null)
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
                directionalLight.GetComponent<Light>().color = convertToFilterColour(shallowWaterColor);
                break;
            case WaterColour.deep:
                waterManager.InitiateWaterTileColourChangeTo(deepWaterColor, waterColourChangeSpeed);
                directionalLight.GetComponent<Light>().color = convertToFilterColour(deepWaterColor);
                break;
            case WaterColour.polluted:
                waterManager.InitiateWaterTileColourChangeTo(pollutedWaterColor, waterColourChangeSpeed);
                directionalLight.GetComponent<Light>().color = convertToFilterColour(pollutedWaterColor);
                break;
            case WaterColour.apocalyptic:
                waterManager.InitiateWaterTileColourChangeTo(apocalypticWaterColor, waterColourChangeSpeed);
                directionalLight.GetComponent<Light>().color = convertToFilterColour(apocalypticWaterColor);
                break;
            case WaterColour.purpleApocalypse:
                waterManager.InitiateWaterTileColourChangeTo(new Color(0, 0.83f, 1, 0.47f), waterColourChangeSpeed);
                directionalLight.GetComponent<Light>().color = purpleApocalypseColor;
                break;
            case WaterColour.black:
                waterManager.InitiateWaterTileColourChangeTo(blackWaterColor, waterColourChangeSpeed);
                directionalLight.GetComponent<Light>().color = convertToFilterColour(blackWaterColor);
                break;
        }
    }

    private void ManageWaterLevel()
    {
        float currentWaterLevel = waterManager.transform.position.y;
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

            waterManager.transform.position = new Vector3(waterManager.transform.position.x, currentWaterLevel, waterManager.transform.position.z);
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
            /*if (powerStationFog.activeSelf)
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
            }*/
        } else if (smokeScale.x < 1)
        {
            powerStationSmoke.SetActive(true);
            float growAmount = shrinkSpeed * Time.deltaTime;
            powerStationSmoke.transform.localScale =
                new Vector3(smokeScale.x + growAmount, smokeScale.y + growAmount, smokeScale.z + growAmount);
        }
    }

    private void ManageLocalFog()
    {
        float desiredFogDistance = fogIntensity * 1000;
        float currentfogDistance = localFog.parameters.distanceFadeEnd;

        if (desiredFogDistance != currentfogDistance)
        {
            float fogLevelDifference = desiredFogDistance - currentfogDistance;

            if (Mathf.Abs(fogLevelDifference) < fogChangeSpeed * Time.deltaTime)
            {
                currentfogDistance = desiredFogDistance;
            }
            else
            {
                currentfogDistance += Mathf.Sign(fogLevelDifference) * fogChangeSpeed * Time.deltaTime;
            }

            localFog.parameters.distanceFadeEnd = currentfogDistance;
        }
    }

    private Color convertToFilterColour(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return Color.HSVToRGB(H, 0.1f, V);
    }
}
