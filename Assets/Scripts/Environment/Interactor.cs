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

    //public Material blackRubber;
    //public Material glow;

    void Outline(GameObject seenObject)
    {
        if (previouslyOutlined != seenObject)
        {
            ClearOutline();
            originalMaterial = seenObject.GetComponent<MeshRenderer>().sharedMaterial;
            seenObject.GetComponent<MeshRenderer>().sharedMaterial = outlineMaterial;
            //seenObject.GetComponent<MeshRenderer>().SetMaterials();
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

    void Update()
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
            Outline(hitInfo.collider.gameObject);

            if (Input.GetKeyDown(KeyCode.I))
            {
                interactiveObject.Interact();
            }
        }
        else
        {
            ClearOutline();
        }
    }
}
