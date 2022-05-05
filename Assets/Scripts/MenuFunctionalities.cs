using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctionalities : MonoBehaviour
{
    public void PlayGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    
    public void QuitGame() {
        Debug.Log("Exited");
        Application.Quit();
    }
}
