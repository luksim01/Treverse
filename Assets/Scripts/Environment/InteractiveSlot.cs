using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSlot : MonoBehaviour, IInteractive
{
    public bool isConnectorAdded = false;

    public void Interact()
    {
        isConnectorAdded = true;
    }
}