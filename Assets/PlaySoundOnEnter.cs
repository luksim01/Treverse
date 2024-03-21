using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnExit : MonoBehaviour
{
    AudioSource source;
    BoxCollider soundTrigger;

    public float timer;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        soundTrigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;   
            source.Play();
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
       
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}