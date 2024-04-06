using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyLightController : MonoBehaviour
{
    // display
    private Material energyLightMaterial;

    // display glow options
    private enum DisplayGlow
    {
        pulsing,
        flickering
    }

    [SerializeField] private DisplayGlow displayGlow;

    // pulsing glow
    [SerializeField] private float pulseGlowSpeed = 1200f;
    [SerializeField] private float minimumGlowLevel = 200f;
    [SerializeField] private float maximumGlowLevel = 1400f;

    // flickering glow
    private bool isFlickering = false;
    [SerializeField] private float flickerGlowSpeed = 30000f;
    [SerializeField] private float stableGlowSpeed = 300f;
    [SerializeField] private float minimumStableGlowLevel = 1100f;
    [SerializeField] private float maximumStableGlowLevel = 1400f;

    void Start()
    {
        List<Material> energyCubeMaterials = new List<Material>();

        // display material
        gameObject.GetComponent<Renderer>().GetMaterials(energyCubeMaterials);
        foreach (Material energyCubeMaterial in energyCubeMaterials)
        {
            if (energyCubeMaterial.name.Contains("EnergyGlowMaterial"))
            {
                energyLightMaterial = energyCubeMaterial;
            }
        }

        // display, flicker mode, stable glow time
        Invoke("StableGlow", 3);
    }

    void FixedUpdate()
    {
        switch (displayGlow)
        {
            case DisplayGlow.pulsing:
                // gradual pulsing glow
                pulseGlowSpeed = PulsingGlow(energyLightMaterial, minimumGlowLevel, maximumGlowLevel, pulseGlowSpeed);
                break;
            case DisplayGlow.flickering:
                if (isFlickering)
                {
                    // fast pulsing glow creates flicker
                    flickerGlowSpeed = PulsingGlow(energyLightMaterial, minimumGlowLevel, maximumGlowLevel, flickerGlowSpeed);
                }
                else
                {
                    // slow pulsing glow creates stable glow
                    stableGlowSpeed = PulsingGlow(energyLightMaterial, minimumStableGlowLevel, maximumStableGlowLevel, stableGlowSpeed);
                }
                break;
        }
    }

    void StableGlow()
    {
        isFlickering = true;
        StartCoroutine(FlickerGlow());

        float stableTime = Random.Range(3, 6);
        Invoke("StableGlow", stableTime);
    }

    IEnumerator FlickerGlow()
    {
        float flickerTime = Random.Range(0.2f, 0.6f);
        yield return new WaitForSeconds(flickerTime);
        isFlickering = false;
    }

    private float PulsingGlow(Material displayMaterial, float minimumGlowLevel, float maximumGlowLevel, float changeSpeed)
    {
        float glowSaturation = displayMaterial.GetFloat("_GlowSaturation") + (changeSpeed * Time.deltaTime);
        
        if (glowSaturation > maximumGlowLevel)
        {
            changeSpeed = -changeSpeed;
            glowSaturation = maximumGlowLevel;
        }
        else if (glowSaturation < minimumGlowLevel)
        {
            changeSpeed = -changeSpeed;
            glowSaturation = minimumGlowLevel;
        }

        energyLightMaterial.SetFloat("_GlowSaturation", glowSaturation);
        //gameObject.GetComponent<Renderer>().material = energyLightMaterial;

        return changeSpeed;
    }
}
