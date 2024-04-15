using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAudio : MonoBehaviour
{
    public static FMOD.Studio.EventInstance NarratorCube;

    
    public static int cubeSustainingCount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        NarratorCube = FMODUnity.RuntimeManager.CreateInstance("event:/Narration/Narration_3");
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }

 

    public void AudioSustainPlayback()
    {
        FMOD.Studio.PLAYBACK_STATE PbState;
        NarratorCube.getPlaybackState(out PbState);

        if (cubeSustainingCount == 0)
        {

            NarratorCube.start();
            cubeSustainingCount++;

        }
        // once audio in playing pause at first sustain marker
        else if (cubeSustainingCount == 1)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)
            {
                // Debug.Log("Voice is currently paused");
                NarratorCube.keyOff();
                Debug.Log("Cue 1");
                cubeSustainingCount++;

            }
        }
        else if (cubeSustainingCount == 2)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)
            {
                NarratorCube.keyOff();
                cubeSustainingCount++;
                Debug.Log("Play");
            }
            
        }
        
        // I GOT IT WORKING OMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMG
    }
}
