using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleGenerator : MonoBehaviour
{

    public List<AudioClip> paddleSounds = new List<AudioClip>();

    public AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
  
}
