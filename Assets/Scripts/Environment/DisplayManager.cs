using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    public Image lowBatteryDisplay;
    private Material lowBatteryDisplayMaterial;
    private float glowSaturation;
    public float changeSpeed = 200f;

    void Start()
    {
        lowBatteryDisplayMaterial = lowBatteryDisplay.material; // luksim not working
        glowSaturation = lowBatteryDisplayMaterial.GetFloat("_GlowSaturation");
        Debug.Log("glowSaturation " + glowSaturation);
    }

    void Update()
    {
        glowSaturation += changeSpeed * Time.deltaTime;
        lowBatteryDisplayMaterial.SetFloat("_GlowSaturation", glowSaturation);

        if(glowSaturation >= 1400 || glowSaturation <= 200)
        {
            changeSpeed = -changeSpeed;
        }
    }
}
