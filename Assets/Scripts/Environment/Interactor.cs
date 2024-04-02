using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractive
{
    public void Interact();
    public ObjectStatus GetObjectStatus();
    public void SetObjectStatus(ObjectStatus objectStatus);
}

public enum ObjectStatus
{
    active,
    inactive,
    destroyed
}

public class Interactor : MonoBehaviour, IDestruct, IInteractor
{
    public Transform lineOfSightSource;
    public float sightRadius;
    public float sightRange;

    public Material outlineMaterial;
    Material objectToOutlineOriginalMaterial;
    public Material glowingOutline;
    public GameObject previouslySeen = null;

    List<GameObject> seenObjectList = new List<GameObject>();
    List<GameObject> outlinedObjectList = new List<GameObject>();
    List<GameObject> destroyObjectList = new List<GameObject>();

    Dictionary<GameObject, GameObject> connectorSlotPairs = new Dictionary<GameObject, GameObject>();

    // pulsing glow
    [SerializeField] private float pulseGlowSpeed = 70f;
    [SerializeField] private float minimumGlowLevel = 10f;
    [SerializeField] private float maximumGlowLevel = 80f;

    void AddOutline(List<GameObject> seenObjectList)
    {
        for (int i = 0; i < seenObjectList.Count; i++)
        {
            GameObject objectToOutline = seenObjectList[i];

            if (!outlinedObjectList.Contains(objectToOutline))
            {
                List<Material> objectToOutlineMaterials = new List<Material>();
                objectToOutline.GetComponent<MeshRenderer>().GetMaterials(objectToOutlineMaterials);

                objectToOutlineOriginalMaterial = objectToOutlineMaterials[0];

                objectToOutlineMaterials.Add(glowingOutline);
                objectToOutline.GetComponent<MeshRenderer>().SetMaterials(objectToOutlineMaterials);

                outlinedObjectList.Add(objectToOutline);
            }
        }
    }

    void RemoveOutline(List<GameObject> outlineObjectList)
    {
        for (int i = 0; i < outlineObjectList.Count; i++)
        {
            GameObject objectToClear = outlineObjectList[i];

            if (outlineObjectList.Contains(objectToClear))
            {
                objectToClear.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { objectToOutlineOriginalMaterial });
            }
        }

        outlineObjectList.Clear();
    }

    void CheckHierarchyForSiblings(GameObject seenObject, out bool hasSiblings, out GameObject parentObject)
    {
        parentObject = seenObject.transform.parent.gameObject;
        if (seenObject.name != parentObject.name)
        {
            hasSiblings = true;
        }
        else
        {
            hasSiblings = false;
        }
    }

    public void AddObjectToDestroy(GameObject objectToDestroy)
    {
        destroyObjectList.Add(objectToDestroy);
    }

    public void AddConnectorSlotPair(GameObject connector, GameObject slot)
    {
        connectorSlotPairs.Add(connector, slot);
        Debug.Log("AddConnectorSlotPair");
        PrintDictionaryPairs(connectorSlotPairs);
    }

    public void RemoveConnectorSlotPair(List<GameObject> connectorSlotPairRemoveList)
    {
        foreach (GameObject connector in connectorSlotPairRemoveList)
        {
            connectorSlotPairs.Remove(connector);
        }

        Debug.Log("RemoveConnectorSlotPair");
        PrintDictionaryPairs(connectorSlotPairs);
    }

    public Dictionary<GameObject, GameObject> GetConnectorSlotPairs()
    {
        return connectorSlotPairs;
    }

    void ManageInteractorVision()
    {
        Ray lineOfSight = new Ray(lineOfSightSource.position, lineOfSightSource.forward);
        bool hasSeenObject = Physics.SphereCast(lineOfSight, sightRadius, out RaycastHit hitInfo, sightRange);
        GameObject seenObject = null;
        bool isInteractiveObject = false;
        bool hasSiblings = false;

        IInteractive interactiveObject = null;
        GameObject parentObject = null;

        ObjectStatus objectStatus = new ObjectStatus();

        if (hasSeenObject)
        {
            isInteractiveObject = hitInfo.collider.gameObject.TryGetComponent(out interactiveObject);
            seenObject = hitInfo.collider.gameObject;

            objectStatus = interactiveObject.GetObjectStatus();
        }

        if (objectStatus == ObjectStatus.active)
        {
            if (hasSeenObject)
            {
                previouslySeen = seenObject;

                if (!seenObjectList.Contains(previouslySeen))
                {
                    RemoveOutline(outlinedObjectList);
                    seenObjectList.Clear();
                }

                if (!seenObjectList.Contains(seenObject))
                {
                    seenObjectList.Add(seenObject);
                    if (seenObject.transform.parent != null)
                    {
                        CheckHierarchyForSiblings(seenObject, out hasSiblings, out parentObject);
                    }
                }
            }

            if (hasSiblings)
            {
                for (int i = 0; i < parentObject.transform.childCount; i++)
                {
                    GameObject siblingObject = parentObject.transform.GetChild(i).gameObject;

                    if (!seenObjectList.Contains(siblingObject))
                    {
                        seenObjectList.Add(siblingObject);
                    }
                }
            }

            //Debug.DrawRay(lineOfSightSource.position, lineOfSightSource.forward * sightRange, Color.red);

            if (hasSeenObject && isInteractiveObject)
            {
                AddOutline(seenObjectList);
                OutlineGlowSettings(outlinedObjectList);

                if (Input.GetKeyDown(KeyCode.I))
                {
                    interactiveObject.Interact();
                }
            }
            else
            {
                RemoveOutline(outlinedObjectList);
                seenObjectList.Clear();
            }
        }
        else if (objectStatus == ObjectStatus.destroyed || objectStatus == ObjectStatus.inactive)
        {
            RemoveOutline(outlinedObjectList);
            outlinedObjectList.Clear();
            seenObjectList.Clear();
        }
    }

    void Update()
    {
        foreach(GameObject objectToDestroy in destroyObjectList)
        {
            Destroy(objectToDestroy);
        }

        ManageInteractorVision();
    }

    private void OutlineGlowSettings(List<GameObject> outlineObjectList)
    {
        // glowing outline pulse
        for (int i = 0; i < outlineObjectList.Count; i++)
        {
            GameObject objectOutlined = outlineObjectList[i];

            List<Material> glowingObjectMaterials = new List<Material>();
            objectOutlined.GetComponent<MeshRenderer>().GetMaterials(glowingObjectMaterials);
            Material glowingOutlineMaterial = glowingObjectMaterials[glowingObjectMaterials.Count - 1];
            pulseGlowSpeed = PulsingGlow(glowingOutlineMaterial, minimumGlowLevel, maximumGlowLevel, pulseGlowSpeed);
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

    // debug methods
    void PrintMaterials(List<Material> materialsList)
    {
        for (int i = 0; i < materialsList.Count; i++)
        {
            Debug.Log("materialsList[" + i + "]: " + materialsList[i]);
        }
    }

    void PrintDictionaryPairs(Dictionary<GameObject, GameObject> keyValuePairs)
    {
        foreach (KeyValuePair<GameObject, GameObject> keyValuePair in keyValuePairs)
        {
            Debug.LogFormat("Slot: {0} Connector: {1}", keyValuePair.Key, keyValuePair.Value);
        }
    }
}
