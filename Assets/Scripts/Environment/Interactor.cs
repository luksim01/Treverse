using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractive
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform lineOfSightSource;
    public float sightRadius;
    public float sightRange;

    public Material outlineMaterial;
    Material originalMaterial;
    GameObject previouslyOutlined;

    public Material glowingOutline;

    // pulsing glow
    [SerializeField] private float pulseGlowSpeed = 70f;
    [SerializeField] private float minimumGlowLevel = 10f;
    [SerializeField] private float maximumGlowLevel = 80f;

    private void Start()
    {
    }

    void Outline(GameObject seenObject)
    {
        if (previouslyOutlined != seenObject)
        {
            ClearOutline();
            originalMaterial = seenObject.GetComponent<MeshRenderer>().sharedMaterial;
            seenObject.GetComponent<MeshRenderer>().sharedMaterial = outlineMaterial;
            previouslyOutlined = seenObject;
        }

    }

    void ClearOutline()
    {
        if (previouslyOutlined != null)
        {
            previouslyOutlined.GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
            previouslyOutlined = null;
        }
    }

    void AddOutline(GameObject seenObject)
    {
        if (previouslyOutlined != seenObject)
        {
            RemoveOutline();
            List<Material> seenObjectMaterials = new List<Material>();
            seenObject.GetComponent<MeshRenderer>().GetMaterials(seenObjectMaterials);

            originalMaterial = seenObjectMaterials[0];

            seenObjectMaterials.Add(glowingOutline);
            seenObject.GetComponent<MeshRenderer>().SetMaterials(seenObjectMaterials);
            previouslyOutlined = seenObject;
        }
    }

    void PrintMaterials(List<Material> materialsList)
    {
        for (int i = 0; i < materialsList.Count; i++)
        {
            Debug.Log("materialsList[" + i + "]: " + materialsList[i]);
        }
    }

    void RemoveOutline()
    {
        if (previouslyOutlined != null)
        {
            previouslyOutlined.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { originalMaterial });
            previouslyOutlined = null;
        }
    }

    void FixedUpdate()
    {
        Ray lineOfSight = new Ray(lineOfSightSource.position, lineOfSightSource.forward);
        bool hasSeenObject = Physics.SphereCast(lineOfSight, sightRadius, out RaycastHit hitInfo, sightRange);
        bool isInteractiveObject = false;
        IInteractive interactiveObject = null;

        if (hasSeenObject)
        {
            isInteractiveObject = hitInfo.collider.gameObject.TryGetComponent(out interactiveObject);
        }

        //Debug.DrawRay(lineOfSightSource.position, lineOfSightSource.forward * sightRange, Color.red);

        if (hasSeenObject && isInteractiveObject)
        {
            GameObject seenObject = hitInfo.collider.gameObject;
            AddOutline(seenObject);


            // glowwing outline pulse
            List<Material> seenObjectMaterials = new List<Material>();
            seenObject.GetComponent<MeshRenderer>().GetMaterials(seenObjectMaterials);
            Material glowingOutlineMaterial = seenObjectMaterials[seenObjectMaterials.Count-1];
            pulseGlowSpeed = PulsingGlow(glowingOutlineMaterial, minimumGlowLevel, maximumGlowLevel, pulseGlowSpeed);

            if (Input.GetKeyDown(KeyCode.I))
            {
                interactiveObject.Interact();
            }
        }
        else
        {
            RemoveOutline();
        }        
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

        return changeSpeed;
    }
}
