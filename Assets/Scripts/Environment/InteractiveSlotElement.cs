using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSlotElement : MonoBehaviour, IInteractiveObject
{
    private InteractiveObjectStatus slotStatus;

    private KayakController kayakController;

    void Start()
    {
        kayakController = GameObject.Find("Kayak").GetComponent<KayakController>();
    }

    public void Interact()
    {
        if (kayakController.energyCubeCount > 0)
        {
            slotStatus = InteractiveObjectStatus.inactive;
        }
    }

    public InteractiveObjectStatus GetObjectStatus()
    {
        return slotStatus;
    }

    public void SetObjectStatus(InteractiveObjectStatus objectStatus)
    {
        slotStatus = objectStatus;
    }
}