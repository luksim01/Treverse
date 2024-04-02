using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractiveObject
{
    public void Interact();
    public InteractiveObjectStatus GetObjectStatus();
    public void SetObjectStatus(InteractiveObjectStatus interactiveObjectStatus);
}

public enum InteractiveObjectStatus
{
    active,
    inactive,
    destroyed
}

interface IInteractor
{
    public void AddObjectToDestroy(GameObject objectToDestroy);
    public void AddConnectorSlotPair(GameObject connector, GameObject slot);
    public void RemoveConnectorSlotPair(List<GameObject> connectorSlotPairRemoveList);
    public Dictionary<GameObject, GameObject> GetConnectorSlotPairs();
}

public class Interactor : MonoBehaviour, IInteractor
{
    // interactivity sight
    public Transform lineOfSightSource;
    public float sightRadius;
    public float sightRange;

    // glowing outline
    private Material objectToOutlineOriginalMaterial;
    public Material glowingOutline;
    private GameObject previouslySeen = null;

    List<GameObject> seenObjectList = new List<GameObject>();
    List<GameObject> outlinedObjectList = new List<GameObject>();
    List<GameObject> destroyObjectList = new List<GameObject>();

    // pulsing glow
    [SerializeField] private float pulseGlowSpeed = 70f;
    [SerializeField] private float minimumGlowLevel = 10f;
    [SerializeField] private float maximumGlowLevel = 80f;

    // connector and slot link
    Dictionary<GameObject, GameObject> connectorSlotPairs = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        DestroyMarkedObjects();
        ManageInteractorVision();
    }

    void DestroyMarkedObjects()
    {
        foreach (GameObject objectToDestroy in destroyObjectList)
        {
            Destroy(objectToDestroy);
        }
    }

    void ManageInteractorVision()
    {
        // vision
        Ray lineOfSight = new Ray(lineOfSightSource.position, lineOfSightSource.forward);
        bool hasSeenObject = Physics.SphereCast(lineOfSight, sightRadius, out RaycastHit hitInfo, sightRange);

        // interactivity
        bool isInteractiveObject = false;
        IInteractiveObject interactiveObject = null;

        // object info
        GameObject seenObject = null;
        InteractiveObjectStatus objectStatus = InteractiveObjectStatus.inactive;

        if (hasSeenObject)
        {
            isInteractiveObject = hitInfo.collider.gameObject.TryGetComponent(out interactiveObject);
            seenObject = hitInfo.collider.gameObject;

            objectStatus = interactiveObject.GetObjectStatus();
        }

        if (objectStatus == InteractiveObjectStatus.active)
        {
            // object hierarchy info
            bool hasSiblings = false;
            GameObject parentObject = null;

            if (hasSeenObject)
            {
                previouslySeen = seenObject;

                // seen new interactive object, remove currently outlined objects
                if (!seenObjectList.Contains(previouslySeen))
                {
                    RemoveOutline(outlinedObjectList);
                    seenObjectList.Clear();
                }

                // seen new interactive object, find hierarchy
                if (!seenObjectList.Contains(seenObject))
                {
                    seenObjectList.Add(seenObject);
                    if (seenObject.transform.parent != null)
                    {
                        CheckHierarchyForSiblings(seenObject, out hasSiblings, out parentObject);
                    }
                }
            }

            // use hierarchy to identify objects to outline
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

            // active interactive objects in vision are outlined
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
        else if (objectStatus == InteractiveObjectStatus.destroyed || objectStatus == InteractiveObjectStatus.inactive)
        {
            RemoveOutline(outlinedObjectList);
            outlinedObjectList.Clear();
            seenObjectList.Clear();
        }
    }

    void AddOutline(List<GameObject> seenObjectList)
    {
        for (int i = 0; i < seenObjectList.Count; i++)
        {
            GameObject objectToOutline = seenObjectList[i];

            // objects that haven't been outlined
            if (!outlinedObjectList.Contains(objectToOutline))
            {
                List<Material> objectToOutlineMaterials = new List<Material>();
                // get current materials list
                objectToOutline.GetComponent<MeshRenderer>().GetMaterials(objectToOutlineMaterials);

                // retain original material, note: only uses first material of object
                objectToOutlineOriginalMaterial = objectToOutlineMaterials[0];

                // add glowing outline to materials list
                objectToOutlineMaterials.Add(glowingOutline);

                // set object materials list
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

    // interface methods
    public void AddObjectToDestroy(GameObject objectToDestroy)
    {
        destroyObjectList.Add(objectToDestroy);
    }

    public void AddConnectorSlotPair(GameObject connector, GameObject slot)
    {
        connectorSlotPairs.Add(connector, slot);
    }

    public void RemoveConnectorSlotPair(List<GameObject> connectorSlotPairRemoveList)
    {
        foreach (GameObject connector in connectorSlotPairRemoveList)
        {
            connectorSlotPairs.Remove(connector);
        }
    }

    public Dictionary<GameObject, GameObject> GetConnectorSlotPairs()
    {
        return connectorSlotPairs;
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

    void DrawSightLine()
    {
        Debug.DrawRay(lineOfSightSource.position, lineOfSightSource.forward * sightRange, Color.red);
    }
}
