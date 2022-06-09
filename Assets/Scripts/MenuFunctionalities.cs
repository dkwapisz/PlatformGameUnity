using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctionalities : MonoBehaviour
{
    [SerializeField] private AudioSource clickSoundEffect;
    
    public void PlayLevel1() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void PlayLevel2() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }

    public void PlayLevel3() { //IN PROGRESS
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }
    
    public void QuitGame() {
        Debug.Log("Exited");
        Application.Quit();
    }
}
