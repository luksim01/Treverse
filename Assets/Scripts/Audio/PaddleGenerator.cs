using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleGenerator : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference paddleAudio;
    private FMOD.Studio.EventInstance paddleLoop;

    private void Awake()
    {
        if(!paddleAudio.IsNull)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(paddleLoop, GetComponent<Transform>(), GetComponent<Rigidbody>());
            
        }
    }
    public void PlayPaddleSounds()
    {
        if (paddleLoop.isValid())
        {
            paddleLoop.start();
        }
    }
}
