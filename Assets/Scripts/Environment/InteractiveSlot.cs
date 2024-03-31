using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSlot : MonoBehaviour, IInteractive
{
    public ObjectStatus slotStatus;

    public void Interact()
    {
        slotStatus = ObjectStatus.inactive;
    }

    public ObjectStatus GetObjectStatus()
    {
        return slotStatus;
    }

    public void SetObjectStatus(ObjectStatus objectStatus)
    {
        slotStatus = objectStatus;
    }
}