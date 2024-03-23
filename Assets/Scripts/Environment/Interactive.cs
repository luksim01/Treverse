using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour, IInteractive
{
    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));
    }
}
