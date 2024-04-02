using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSlotElement : MonoBehaviour, IInteractiveObject
{
    private InteractiveObjectStatus slotStatus;

    public void Interact()
    {
        slotStatus = InteractiveObjectStatus.inactive;
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