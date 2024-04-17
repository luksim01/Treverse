using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 2f;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadMainScene();
        }
    }

    public void LoadMainScene()
    {
        StartCoroutine(MainAnimation(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator MainAnimation(int levelIndex) 
    {

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
