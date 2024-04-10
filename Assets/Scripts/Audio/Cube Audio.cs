using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAudio : MonoBehaviour
{
    public FMOD.Studio.EventInstance NarratorCube;

    
    private int cubeSustainingCount = 0;


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
        AudioSustainPlayback();
    }

    // When cube is destroyed play 

    private void FirstNarrationCue()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            NarratorCube.keyOff();
            //  Debug.Log("Cue 1");
            cubeSustainingCount++;
        }
    }

    private void SecondNarrationCue()
    {



        if (Input.GetKeyDown(KeyCode.P))
        {
            NarratorCube.keyOff();
            //    Debug.Log("Cue 2");
            cubeSustainingCount++;
        }

    }

    private void AudioSustainPlayback()
    {
        FMOD.Studio.PLAYBACK_STATE PbState;
        NarratorCube.getPlaybackState(out PbState);

        // once audio in playing pause at first sustain marker
        if (cubeSustainingCount == 0)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)
            {
                // Debug.Log("Voice is currently paused");
                FirstNarrationCue();

            }
        }
        if (cubeSustainingCount == 1)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {

                cubeSustainingCount++;

            }
        }
        if (cubeSustainingCount == 2)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)
            {

                SecondNarrationCue();
            }
        }
        if (cubeSustainingCount == 3)
        {

        }
        // I GOT IT WORKING OMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMG
    }
}
