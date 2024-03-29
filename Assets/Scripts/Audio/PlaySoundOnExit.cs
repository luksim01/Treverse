using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnExit : MonoBehaviour
{

    FMODUnity.StudioEventEmitter guitarEmitterRef;

    BoxCollider soundTrigger;

    public float timer;

    private void Awake()
    {
       guitarEmitterRef = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void OnTriggerExit(Collider other)
    {
    
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            guitarEmitterRef.Play();
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
       
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}