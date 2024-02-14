using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public Animator camAnimation;
    public GameObject about;
    // Loads main game scene
    public void playMainGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        Debug.Log("Play");
    }

    public void aboutTheGame()
    {
        camAnimation.SetTrigger("About");
        about.SetActive(true);

        Debug.Log("About");
      
    }

    public void backtoMenu()
    {
        camAnimation.SetTrigger("Return");
        about.SetActive(false);
    }

    // Coroutine for transtions

    IEnumerator LoadLevel(int levelIndex)
    {
  
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
       
    }
}
