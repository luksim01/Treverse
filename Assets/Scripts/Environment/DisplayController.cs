using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayController : MonoBehaviour
{
    // display
    public Image displayImage;
    private Material displayImageMaterial;

    // display glow options
    private enum DisplayGlow
    {
        pulsing,
        flickering
    }

    [SerializeField] private DisplayGlow displayGlow;

    // pulsing glow
    private float pulseGlowSpeed = 1200f;
    private float minimumGlowLevel = 200f;
    private float maximumGlowLevel = 1400f;

    // flickering glow
    private bool isFlickering = false;
    private float flickerGlowSpeed = 30000f;
    private float stableGlowSpeed = 300f;
    private float minimumStableGlowLevel = 1100f;
    private float maximumStableGlowLevel = 1400f;    

    void Start()
    {
        // display material
        displayImageMaterial = new Material(displayImage.material);

        // display, flicker mode, stable glow time
        Invoke("StableGlow", 3);
    }

    void FixedUpdate()
    {
        switch (displayGlow)
        {
            case DisplayGlow.pulsing:
                // gradual pulsing glow
                pulseGlowSpeed = PulsingGlow(displayImageMaterial, minimumGlowLevel, maximumGlowLevel, pulseGlowSpeed);
                break;
            case DisplayGlow.flickering:
                if (isFlickering)
                {
                    // fast pulsing glow creates flicker
                    flickerGlowSpeed = PulsingGlow(displayImageMaterial, minimumGlowLevel, maximumGlowLevel, flickerGlowSpeed);
                }
                else
                {
                    // slow pulsing glow creates stable glow
                    stableGlowSpeed = PulsingGlow(displayImageMaterial, minimumStableGlowLevel, maximumStableGlowLevel, stableGlowSpeed);
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

        displayMaterial.SetFloat("_GlowSaturation", glowSaturation);
        displayImage.material = displayMaterial;

        return changeSpeed;
    }
}
