using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnExit : MonoBehaviour
{
    
    BoxCollider soundTrigger;

    public float timer;

    private void Awake()
    {

       
        soundTrigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;   
            
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
       
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}