using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorTrigger : MonoBehaviour
{

    public FMOD.Studio.EventInstance Narrator;

    public GameObject droneInpurtUI;

    private int sustainingCount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        Narrator = FMODUnity.RuntimeManager.CreateInstance("event:/Narration/Narration_1");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        FMOD.Studio.PLAYBACK_STATE PbState;
        Narrator.getPlaybackState(out PbState);

        // once audio in playing pause at first sustain marker
        if (sustainingCount == 0)
        { 
            if (PbState == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)
            {
                Debug.Log("Voice is currently paused");
                FirstNarrationCue();


            }
        }
        if (sustainingCount == 1)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                
              sustainingCount++;

            }
        }
        if(sustainingCount == 2)
        {
            if (PbState == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)
            {
                droneInpurtUI.SetActive(true);
                SecondNarrationCue();
            }
        }
        // I GOT IT WORKING OMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMGOMG
    }


    private void OnTriggerExit(Collider other)
    {
        // when player exits trigger, play
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            Narrator.start();
            Debug.Log("Play");
        
        }

        
    }

    private void FirstNarrationCue()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            Narrator.keyOff();
            Debug.Log("Cue 1");
            sustainingCount++;
        }
    }

    private void SecondNarrationCue()
    {

            
        
            if (Input.GetKeyDown(KeyCode.P))
            {
                Narrator.keyOff();
                Debug.Log("Cue 2");
            }
        
    }
}
